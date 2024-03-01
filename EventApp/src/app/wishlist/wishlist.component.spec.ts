import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { WishlistComponent } from './wishlist.component';
import { EventserviceService } from '../shared/eventservice.service';
import { Router } from '@angular/router';
import { of, throwError } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { LoggingService } from '../shared/logging.service';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { RouterTestingModule } from '@angular/router/testing';

describe('WishlistComponent', () => {

 let component: WishlistComponent;
 let fixture: ComponentFixture<WishlistComponent>;
 let eventServiceSpy: jasmine.SpyObj<EventserviceService>;
 let toastrServiceSpy: jasmine.SpyObj<ToastrService>;
 let loggingServiceSpy: jasmine.SpyObj<LoggingService>;
 let router: Router;

 beforeEach(async () => {

  eventServiceSpy = jasmine.createSpyObj('EventserviceService', ['GetWishlist', 'DeleteWishlist']);
  toastrServiceSpy = jasmine.createSpyObj('ToastrService', ['success', 'error']);
  loggingServiceSpy = jasmine.createSpyObj('LoggingService', ['logInfo', 'logError']);

  await TestBed.configureTestingModule({
   declarations: [WishlistComponent],
   providers: [
    { provide: EventserviceService, useValue: eventServiceSpy },
    { provide: ToastrService, useValue: toastrServiceSpy },
    { provide: LoggingService, useValue: loggingServiceSpy },
    Router,
   ],
   imports: [FontAwesomeModule, RouterTestingModule],
  }).compileComponents();
  fixture = TestBed.createComponent(WishlistComponent);
  component = fixture.componentInstance; 
});

 it('should create', () => {
  expect(component).toBeTruthy();
 });

 it('should initialize with empty Eventlist and EventsFilter', () => {
  component.Eventlist=[]
  component.EventsFilter=[]
  expect(component.Eventlist).toEqual([]);
  expect(component.EventsFilter).toEqual([]);
 });

 it('should set token when it is present in localStorage', () => {
  const mockEvents={};
  spyOn(localStorage, 'getItem').and.returnValue('testToken');
  eventServiceSpy.GetWishlist.and.returnValue(of({  }));
  spyOn(console, 'log');
  component.ngOnInit();
  expect(component.token).toBe('testToken');
 });

 it('should set token to empty when it is not present in localStorage', () => {
  const mockEvents = [{ id: 1, title: 'Event 1' }];
  eventServiceSpy.GetWishlist.and.returnValue(of({ events: mockEvents }));
  spyOn(console, 'log');
  spyOn(localStorage, 'getItem').and.returnValue(null);
  component.ngOnInit();
  expect(component.token).toBe('');
  expect(component.userName).toBe('');
 });

 it('should retrieve wishlist events on initialization', fakeAsync(() => {
  const mockEvents = [{ id: 1, title: 'Event 1' }, { id: 2, title: 'Event 2' }];
  eventServiceSpy.GetWishlist.and.returnValue(of({ events: mockEvents }));
  spyOn(console, 'log');
  component.userName='testUserName';
  component.token='testToken';
  component.ngOnInit();
  tick();
  expect(component.Eventlist).toEqual(mockEvents);
  expect(component.EventsFilter).toEqual(mockEvents);
  expect(loggingServiceSpy.logInfo).toHaveBeenCalledWith('Successfully received event data. ');
  expect(console.log).toHaveBeenCalledWith('Successfully received event data', mockEvents);
 }));

 it('should handle error while loading wishlist events', fakeAsync(() => {
  const error = new Error('Failed to load wishlist events.');
  component.userName='';
  component.token='';
  eventServiceSpy.GetWishlist.and.returnValue(throwError(()=>error));
  spyOn(console, 'error');
  component.ngOnInit();
  tick();
  expect(loggingServiceSpy.logError).toHaveBeenCalledWith('failed fetching data. ');
  expect(console.error).toHaveBeenCalledWith('failed fetching data', error);
 }));

 it('should filter events based on search criteria', () => {
  component.EventsFilter = [
   { short_title: 'Event A' },
   { short_title: 'Event B' },
   { short_title: 'Event C' },
  ];
  component.OnSearch('B');
  expect(component.Eventlist).toEqual([{ short_title: 'Event B' }]);
 });

 it('should delete event from wishlist successfully', () => {
  eventServiceSpy.DeleteWishlist.and.returnValue(of({}));
  const mockEvents = [{ id: 1, title: 'Event 1' },{ id: 2, title: 'Event 2' }];
  eventServiceSpy.GetWishlist.and.returnValue(of({ events: mockEvents }));
  component.userName='testUserName';
  component.token='testToken';
  spyOn(console, 'log');
  component.deletefromwishlist(1);
  expect(eventServiceSpy.DeleteWishlist).toHaveBeenCalledWith('testUserName', 1);
  expect(loggingServiceSpy.logInfo).toHaveBeenCalledWith('Successfully removed an event.');
  expect(toastrServiceSpy.success).toHaveBeenCalledWith('Removed from wishlist', 'Success');
 });

 it('should handle error when deleting event from wishlist', () => {
  const error = new Error('Error deleting event from wishlist.');
  eventServiceSpy.DeleteWishlist.and.returnValue(throwError(()=>error));
  component.userName='testUserName';
  component.token='testToken';
  spyOn(console, 'error');
  component.deletefromwishlist(1);
  expect(loggingServiceSpy.logError).toHaveBeenCalledWith('Failed to delete data. ');
  expect(console.error).toHaveBeenCalledWith('Failed to delete data', error);
  expect(toastrServiceSpy.error).toHaveBeenCalledWith('Event not found in wishlist', 'Error');
 });
});
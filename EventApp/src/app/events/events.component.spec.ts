import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { EventsComponent } from './events.component';
import { EventserviceService } from '../shared/eventservice.service';
import {  Router } from '@angular/router';
import { of, throwError } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { LoggingService } from '../shared/logging.service';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { RouterTestingModule } from '@angular/router/testing';

describe('EventsComponent', () => {

 let component: EventsComponent;
 let fixture: ComponentFixture<EventsComponent>;
 let eventServiceSpy: jasmine.SpyObj<EventserviceService>;
 let toastrServiceSpy: jasmine.SpyObj<ToastrService>;
 let loggingServiceSpy: jasmine.SpyObj<LoggingService>;

 beforeEach(async () => {
  eventServiceSpy = jasmine.createSpyObj('EventserviceService', ['GetEvents', 'AddToWishlist']);
  toastrServiceSpy = jasmine.createSpyObj('ToastrService', ['success', 'warning']);
  loggingServiceSpy = jasmine.createSpyObj('LoggingService', ['logInfo', 'logError', 'logWarn']);

  await TestBed.configureTestingModule({
   declarations: [EventsComponent],
   providers: [
    { provide: EventserviceService, useValue: eventServiceSpy },
    { provide: ToastrService, useValue: toastrServiceSpy },
    { provide: LoggingService, useValue: loggingServiceSpy }, // Include dependencies
    Router,
   ],
   imports: [FontAwesomeModule,RouterTestingModule],
  }).compileComponents();
 });

 beforeEach(() => {
  fixture = TestBed.createComponent(EventsComponent);
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
 it('should set token when it is present in localStorage',()=>{
  const mockEvents = [{ title: 'Event 1' }, { title: 'Event 2' }];
  eventServiceSpy.GetEvents.and.returnValue(of({ events: mockEvents }));
  spyOn(localStorage,'getItem').and.returnValue('testToken');
  component.ngOnInit();
  expect(component.token).toBe('testToken');
});
it('should set token to empty when it is not present in localStorage',()=>{
  const mockEvents = [{ title: 'Event 1' }, { title: 'Event 2' }];
  eventServiceSpy.GetEvents.and.returnValue(of({ events: mockEvents }));
  spyOn(localStorage,'getItem').and.returnValue(null);
  component.ngOnInit();
  expect(component.token).toBe('');
});

 it('should retrieve events on initialization', fakeAsync(() => {
  const mockEvents = [{ title: 'Event 1' }, { title: 'Event 2' }];
  eventServiceSpy.GetEvents.and.returnValue(of({ events: mockEvents }));
  component.ngOnInit();
  tick();
  expect(component.Eventlist).toEqual(mockEvents);
  expect(component.EventsFilter).toEqual(mockEvents);
  expect(loggingServiceSpy.logInfo).toHaveBeenCalledWith('Successfully received event data.');
 }));

 it('should add event to wishlist successfully', () => {
  const mockEvent = { title: 'Event 1' };
  eventServiceSpy.AddToWishlist.and.returnValue(of({}));
  component.userName = 'testUser';
  component.Addtowishlist(mockEvent);
  expect(eventServiceSpy.AddToWishlist).toHaveBeenCalledWith('testUser', mockEvent);
  expect(loggingServiceSpy.logInfo).toHaveBeenCalledWith('Successfully added event to wishlist.');
  expect(toastrServiceSpy.success).toHaveBeenCalledWith('Added to wishlist', 'Success');
 });

 it('should handle error while loading events', fakeAsync(() => {
  const error = new Error('Failed to load events.');
  eventServiceSpy.GetEvents.and.returnValue(throwError(()=>error));
  component.ngOnInit();
  tick();
  expect(loggingServiceSpy.logError).toHaveBeenCalledWith('Error fetching events.');
  expect(component.Eventlist).toBeUndefined();
}));

 it('should handle failure when adding event to wishlist', () => {
  const mockEvent = { title: 'Event 1' };
  const error={ status: 409 };
  eventServiceSpy.AddToWishlist.and.returnValue(throwError(()=>error));
  component.userName = 'testUser';
  component.Addtowishlist(mockEvent);
  expect(loggingServiceSpy.logWarn).toHaveBeenCalledWith('Failed adding event.');
  expect(toastrServiceSpy.warning).toHaveBeenCalledWith('Event already exists in wishlist', 'Failed');
 });

 it('should filter events based on search criteria', () => {
  component.EventsFilter = [
   { short_title: 'Event A' },
   { short_title: 'Event B' },
   { short_title: 'Event C' },
  ];
  component.OnSearch('B');
  expect(component.Eventlist).toEqual([{ short_title: 'Event B' }]);
 });
});


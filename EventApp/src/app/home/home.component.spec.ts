import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { HomeComponent } from './home.component';
import { EventserviceService } from '../shared/eventservice.service';
import { RouterTestingModule } from '@angular/router/testing';
import { Router } from '@angular/router';
import { of, throwError } from 'rxjs';
import { LoggingService } from '../shared/logging.service';
import { ToastrService } from 'ngx-toastr';

describe('HomeComponent', () => {
  let component: HomeComponent;
  let fixture: ComponentFixture<HomeComponent>;
  let eventServiceSpy: jasmine.SpyObj<EventserviceService>;
  let loggingServiceSpy: jasmine.SpyObj<LoggingService>;
  let toastrServiceSpy: jasmine.SpyObj<ToastrService>;

  beforeEach(async () => {
    eventServiceSpy = jasmine.createSpyObj('EventService', ['GetEvents','AddToWishlist']);
    loggingServiceSpy = jasmine.createSpyObj('LoggingService', ['logInfo', 'logError','logWarn']);
    toastrServiceSpy=jasmine.createSpyObj('ToastrService',['success','error','warning']);
 
    await TestBed.configureTestingModule({
      imports: [RouterTestingModule],
      declarations: [HomeComponent],
      providers: [
        { provide: EventserviceService, useValue: eventServiceSpy },
        { provide: LoggingService, useValue: loggingServiceSpy },
        {provide:ToastrService,useValue:toastrServiceSpy},
        Router, // Inject the Router
      ],
    }).compileComponents();
 
    fixture = TestBed.createComponent(HomeComponent);
    component = fixture.componentInstance;
  });
 
  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should set token when it is present in localStorage',()=>{
      const mockEvents = [{ title: 'Event 1' }, { title: 'Event 2' }];
      eventServiceSpy.GetEvents.and.returnValue(of({ events: mockEvents }));
      spyOn(localStorage,'getItem').and.returnValue('testToken');
      component.ngOnInit();
      component.ngAfterContentChecked();
      expect(component.token).toBe('testToken');
  });
  it('should set token to empty when it is not present in localStorage',()=>{
    const mockEvents = [{ title: 'Event 1' }, { title: 'Event 2' }];
    eventServiceSpy.GetEvents.and.returnValue(of({ events: mockEvents }));
    spyOn(localStorage,'getItem').and.returnValue(null);
    component.ngOnInit();
    component.ngAfterContentChecked();
    expect(component.token).toBe('');
});

  it('should load events on initialization', fakeAsync(() => {
    const mockEvents = [{ title: 'Event 1' }, { title: 'Event 2' }];
    eventServiceSpy.GetEvents.and.returnValue(of({ events: mockEvents }));
    component.ngOnInit();
    tick(); // wait for asynchronous operation to complete
    component.ngAfterContentChecked();
    expect(component.Eventlist).toEqual(mockEvents);
    expect(loggingServiceSpy.logInfo).toHaveBeenCalledWith('Successfully received event data');
  }));
  it('should add event to wishlist', () => {
    const mockEvent = { title: 'Event 1' };
    const mockResponse = { /* your mock response object */ };
    eventServiceSpy.AddToWishlist.and.returnValue(of(mockResponse));
    component.userName = 'testUser';
    component.token = 'token';
    component.Addtowishlist(mockEvent);
    expect(eventServiceSpy.AddToWishlist).toHaveBeenCalledWith('testUser', mockEvent);
  });
 
  it('should navigate to login when adding to wishlist without a user login', () => {
    component.userName = "";
    component.token = "";
    spyOn(component.getRouter(), 'navigate'); // spy on router.navigate method
    component.Addtowishlist({ title: 'Event 1' });
    expect(component.token).toBe('');
    expect(component.userName).toBe('');
    expect(component.getRouter().navigate).toHaveBeenCalledWith(['/login']);
  });

  it('should handle error while loading events', fakeAsync(() => {
    const error = new Error('Failed to load events');
    eventServiceSpy.GetEvents.and.returnValue(throwError(()=>error));
    component.ngOnInit();
    tick();
    expect(loggingServiceSpy.logError).toHaveBeenCalledWith('Error fetching events');
    expect(component.Eventlist).toBeUndefined();
  }));




  it('should handle other errors while adding event to wishlist', () => {
    const mockEvent = { title: 'Event 1' };
    const error = new Error("Failed adding event");
    eventServiceSpy.AddToWishlist.and.returnValue(throwError(()=>error));
    component.userName = 'testUser';
    component.token = 'token';
    component.Addtowishlist(mockEvent);
    expect(eventServiceSpy.AddToWishlist).toHaveBeenCalledWith('testUser', mockEvent);
    expect(loggingServiceSpy.logWarn).toHaveBeenCalledWith('Failed adding event');
    expect(toastrServiceSpy.warning).toHaveBeenCalledWith('Event already exists in wishlist', 'Failed');
  });
  
});

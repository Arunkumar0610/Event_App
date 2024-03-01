import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { EventserviceService } from './eventservice.service';
import { User } from './User';
import { EventItem, Venue } from './EventItem';

describe('EventserviceService', () => {

 let service: EventserviceService;
 let httpTestingController: HttpTestingController;

 beforeEach(() => {
  TestBed.configureTestingModule({
   imports: [HttpClientTestingModule],
   providers: [EventserviceService],
  });
  service = TestBed.inject(EventserviceService);
  httpTestingController = TestBed.inject(HttpTestingController);
 });

 afterEach(() => {
  httpTestingController.verify();
 });

 it('should be created', () => {
  expect(service).toBeTruthy();
 });

 it('should get events', () => {
  const mockEvents = [{ title: 'Event 1' }, { title: 'Event 2' }];
  service.GetEvents().subscribe((response) => {
   expect(response.events).toEqual(mockEvents);
  });
  const req = httpTestingController.expectOne('https://localhost:5000/gateway/Events');
  expect(req.request.method).toEqual('GET');
  req.flush({ events: mockEvents });
 });

 it('should get event by id', () => {
  const mockEventId = 1;
  const mockEvent: EventItem = {
    Short_title: 'Event 1',
    Id: 0,
    Type: '',
    Title: '',
    Venue: new Venue,
    Performers: []
  };
  service.GetEventById(mockEventId).subscribe((response) => {
   expect(response).toEqual(mockEvent);
  });
  const req = httpTestingController.expectOne(`https://localhost:5000/gateway/Events/${mockEventId}`);
  expect(req.request.method).toEqual('GET');
  req.flush(mockEvent);
 });
 
 it('should sign up a user', () => {
  const mockUser: User = { userId: '1', name: 'John', userName: 'john_doe', password: 'password', confirmPassword: 'password', email: 'john@example.com', contact: '1234567890' };
  service.SignUpUser(mockUser).subscribe((response) => {
   expect(response).toBeTruthy(); // Adjust based on your actual response
  });
  const req = httpTestingController.expectOne('https://localhost:5000/gateway/Users');
  expect(req.request.method).toEqual('POST');
  req.flush({ /* mock response */ });
 });

 it('should login a user', () => {
  const mockCredentials = { username: 'john_doe', password: 'password' };
  service.LoginUser(mockCredentials).subscribe((response) => {
   expect(response).toBeTruthy(); // Adjust based on your actual response
  });
  const req = httpTestingController.expectOne('https://localhost:5000/gateway/Authenticate/login');
  expect(req.request.method).toEqual('POST');
  req.flush({ /* mock response */ });
 });

 it('should get wishlist for a user', () => {
  const mockUsername = 'john_doe';
  const mockWishlist = [{ title: 'Event 1' }, { title: 'Event 2' }];
  service.GetWishlist(mockUsername).subscribe((response) => {
   expect(response.events).toEqual(mockWishlist);
  });
  const req = httpTestingController.expectOne(`https://localhost:5000/gateway/Wishlist/${mockUsername}`);
  expect(req.request.method).toEqual('GET');
  req.flush({ events: mockWishlist });
 });

 it('should add event to wishlist for a user', () => {
  const mockUsername = 'john_doe';
  const mockEventItem = { title: 'Event 1' };
  service.AddToWishlist(mockUsername, mockEventItem).subscribe((response) => {
   expect(response).toBeTruthy(); // Adjust based on your actual response
  });
  const req = httpTestingController.expectOne(`https://localhost:5000/gateway/Wishlist/${mockUsername}`);
  expect(req.request.method).toEqual('POST');
  req.flush({ /* mock response */ });
 });

 it('should delete event from wishlist for a user', () => {
  const mockUsername = 'john_doe';
  const mockEventId = 1;
  service.DeleteWishlist(mockUsername, mockEventId).subscribe((response) => {
   expect(response).toBeTruthy(); // Adjust based on your actual response
  });
  const req = httpTestingController.expectOne(`https://localhost:5000/gateway/Wishlist/${mockUsername}/${mockEventId}`);
  expect(req.request.method).toEqual('DELETE');
  req.flush({ /* mock response */ });
 });

});

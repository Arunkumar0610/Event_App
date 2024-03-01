import { ComponentFixture, TestBed } from '@angular/core/testing';
import { NavbarComponent } from './navbar.component';
import { Router } from '@angular/router';
import { AuthenticateService } from 'src/app/shared/authenticate.service';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';

describe('NavbarComponent', () => {

 let component: NavbarComponent;
 let fixture: ComponentFixture<NavbarComponent>;
 let routerSpy: jasmine.SpyObj<Router>;
 let authServiceSpy: jasmine.SpyObj<AuthenticateService>;

 beforeEach(async () => {

  routerSpy = jasmine.createSpyObj('Router', ['navigate']);
  authServiceSpy = jasmine.createSpyObj('AuthenticateService', ['IsloggedIn']);
  await TestBed.configureTestingModule({
   declarations: [NavbarComponent],
   providers: [
    { provide: Router, useValue: routerSpy },
    { provide: AuthenticateService, useValue: authServiceSpy },
   ],
   imports: [FontAwesomeModule],
  }).compileComponents();
  fixture = TestBed.createComponent(NavbarComponent);
  component = fixture.componentInstance;
 });

 it('should create', () => {
  expect(component).toBeTruthy();
 });

 it('should initialize component properties', () => {
  spyOn(localStorage,'getItem').and.returnValue(null);
  authServiceSpy.IsloggedIn.and.returnValue(false); 
  component.ngOnInit();
  expect(component.userName).toBeDefined();
  expect(component.islogin).toBeDefined();
 });

 it('should set islogin and userName correctly after checking content', () => {
  authServiceSpy.IsloggedIn.and.returnValue(true);
  component.ngAfterContentChecked();
  expect(component.islogin).toBe(true);
  expect(component.userName).toBeDefined();
 });

 it('should set islogin and userName correctly after checking content when not logged in', () => {
  authServiceSpy.IsloggedIn.and.returnValue(false);
  component.ngAfterContentChecked();
  expect(component.islogin).toBe(false);
  expect(component.userName).toBeDefined();
 });

 it('should set confirmation to true and success to false when confirmlogout is called', () => {
  component.confirmlogout();
  expect(component.confirmation).toBe(true);
  expect(component.success).toBe(false);
 });

 it('should navigate to home and clear local storage when confirmlogout is called and confirmation is true', () => {
  component.confirmation = true;
  component.confirmlogout();
  expect(component.islogin).toBe(false);
  expect(routerSpy.navigate).toHaveBeenCalledWith(['/home']);
  expect(localStorage.getItem('jwt')).toBe(null);
  expect(localStorage.getItem('userName')).toBe(null);
  expect(localStorage.getItem('event')).toBe(null);
 });

 it('should set confirmation to false when closeconfirmation is called', () => {
  component.closeconfirmation();
  expect(component.confirmation).toBe(false);
 });

 it('should set success to false when closesuccess is called', () => {
  component.closesuccess();
  expect(component.success).toBe(false);
 });

 it('should set success to true when Logout is called', () => {
  component.Logout();
  expect(component.success).toBe(true);
 });
});

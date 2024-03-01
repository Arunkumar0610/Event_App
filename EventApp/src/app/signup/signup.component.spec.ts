import { ComponentFixture, TestBed } from '@angular/core/testing';
import { SignupComponent } from './signup.component';
import { EventserviceService } from '../shared/eventservice.service';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterTestingModule } from '@angular/router/testing';
import { Router } from '@angular/router';
import { of, throwError } from 'rxjs';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ToastrModule, ToastrService } from 'ngx-toastr';
import { LoggingService } from '../shared/logging.service';

describe('SignupComponent', () => {
  let component: SignupComponent;
  let fixture: ComponentFixture<SignupComponent>;
  let eventService: EventserviceService;
  let loggingServiceSpy: jasmine.SpyObj<LoggingService>;
  let toastrServiceSpy: jasmine.SpyObj<ToastrService>;
  let router: any;

  beforeEach(async () => {
    loggingServiceSpy = jasmine.createSpyObj('LoggingService', ['logInfo', 'logError','logWarn']);
    toastrServiceSpy=jasmine.createSpyObj('ToastrService',['success','error','warning']);
    await TestBed.configureTestingModule({
      declarations: [SignupComponent],
      imports: [ReactiveFormsModule, RouterTestingModule,HttpClientTestingModule,ToastrModule],
      providers: [EventserviceService,{provide:ToastrService,useValue:toastrServiceSpy},
        { provide: LoggingService, useValue: loggingServiceSpy },],
    }).compileComponents();
 
    fixture = TestBed.createComponent(SignupComponent);
    component = fixture.componentInstance;
    eventService = TestBed.inject(EventserviceService);
    router = TestBed.inject(Router);
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
 
  it('should have an invalid form on initialization', () => {
    expect(component.form.valid).toBeFalsy();
  });
 
  it('should mark form controls as vaild on form submission', () => {
    // Arrange
    component.form.controls['name'].setValue('John');
    component.form.controls['username'].setValue('john_doe');
    component.form.controls['email'].setValue('john@example.com');
    component.form.controls['password'].setValue('Password@123');
    component.form.controls['confirmpassword'].setValue('Password@123');
    component.form.controls['contact'].setValue('1234567890');
 
    // Act
    component.UserRegistration();
  
    // Assert
    expect(component.form.controls['name'].valid).toBeTruthy();
    expect(component.form.controls['username'].valid).toBeTruthy();
    expect(component.form.controls['email'].valid).toBeTruthy();
    expect(component.form.controls['password'].valid).toBeTruthy();
    expect(component.form.controls['confirmpassword'].valid).toBeTruthy();
    expect(component.form.controls['contact'].valid).toBeTruthy();
  });
 
  it('should navigate to login page on successful registration', () => {
    // Arrange
    spyOn(eventService, 'SignUpUser').and.returnValue(of({ /* mock response */ }));
    spyOn(router, 'navigate');
    // Act
    component.UserRegistration();
    // Assert
    expect(router.navigate).toHaveBeenCalledWith(['/login']);
  });

  it('should handle username conflict error during registration', () => {
    // Arrange
    const error={status:409};
    spyOn(eventService, 'SignUpUser').and.returnValue(throwError(()=>error)); 
    // Act 
    component.UserRegistration(); 
    // Assert
    expect(loggingServiceSpy.logError).toHaveBeenCalledWith('Username already exists.');
    expect(toastrServiceSpy.error).toHaveBeenCalledWith('Username already exists', 'Failed to SignUp');
  
  });
  
  
  
  it('should handle other registration errors', () => {
    // Arrange
    const error={status:500};
    spyOn(eventService, 'SignUpUser').and.returnValue(throwError(()=>error));
   // Act
   component.UserRegistration(); 
    // Assert 
    expect(loggingServiceSpy.logError).toHaveBeenCalledWith('Registration failed. Please try again.');  
    expect(toastrServiceSpy.error).toHaveBeenCalledWith('Registration failed. Please try again.', 'Something went wrong'); 
  });
  
  
  

});

import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { LoginComponent } from './login.component';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { ToastrModule, ToastrService } from 'ngx-toastr';
import { of, throwError } from 'rxjs';
import { EventserviceService } from '../shared/eventservice.service';
import { LoggingService } from '../shared/logging.service';

describe('LoginComponent', () => {
  let component: LoginComponent;
  let fixture: ComponentFixture<LoginComponent>;
  let eventsService: jasmine.SpyObj<EventserviceService>;
  let loggingServiceSpy: jasmine.SpyObj<LoggingService>;
  let toastrService: jasmine.SpyObj<ToastrService>;

  beforeEach(waitForAsync(() => {
    eventsService = jasmine.createSpyObj('EventserviceService', ['LoginUser']);
    loggingServiceSpy = jasmine.createSpyObj('LoggingService', ['logInfo', 'logError']);
    toastrService= jasmine.createSpyObj('ToastrService',['success','error','warning']);
    TestBed.configureTestingModule({
      declarations: [LoginComponent],
      imports: [
        ReactiveFormsModule,
        HttpClientTestingModule,
        RouterTestingModule,
        ToastrModule.forRoot()
      ],
      providers: [
        { provide: EventserviceService, useValue: eventsService },
        { provide: LoggingService, useValue: loggingServiceSpy },
        { provide: ToastrService, useValue: toastrService }
      ]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LoginComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should have a valid form', () => {
    component.form.setValue({ username: 'testuser', password: 'testpassword' });
    expect(component.form.valid).toBeTruthy();
  });

  it('should have invalid form when empty', () => {
    expect(component.form.valid).toBeFalsy();
  });

  it('should call LoginUser on form submission', () => {
    const loginSpy = eventsService.LoginUser.and.returnValue(of({ token: 'testToken', userName: 'testuser' }));
    component.form.setValue({ username: 'testuser', password: 'testpassword' });
    spyOn(component.getRouter(), 'navigate');
    component.Login();
    expect(loginSpy).toHaveBeenCalledWith(jasmine.any(Object));
    expect(component.getRouter().navigate).toHaveBeenCalledWith(['/home']);
    expect(loggingServiceSpy.logInfo).toHaveBeenCalledWith('Navigate to the home page');
    expect(loggingServiceSpy.logInfo).toHaveBeenCalledWith('Token: testToken');
    expect(loggingServiceSpy.logInfo).toHaveBeenCalledWith('UserName: testuser');
  });

  it('should handle login error with invalid credentials', () => {
    const error = {status:401};
    eventsService.LoginUser.and.returnValue(throwError((()=>error)));
    component.form.setValue({ username: 'testuser', password: 'testpassword' });
    component.Login();
    expect(loggingServiceSpy.logError).toHaveBeenCalledWith('Invalid Username or Password');
    expect(toastrService.error).toHaveBeenCalledWith('Invalid Username or Password', 'Invalid Credentials');
  });

  it('should handle login error with generic message', () => {
    const error = new Error("Login failed. Please try again.");
    eventsService.LoginUser.and.returnValue(throwError(()=>error));
    component.form.setValue({ username: 'testuser', password: 'testpassword' });
    component.Login();
    expect(loggingServiceSpy.logError).toHaveBeenCalledWith('Login failed. Please try again.');
    expect(toastrService.error).toHaveBeenCalledWith('Login failed. Please try again.', 'Something went wrong');
  });

});



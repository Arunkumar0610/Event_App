import { TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { AppComponent } from './app.component';
import { NavbarComponent } from './navigation/navbar/navbar.component';
import { AuthenticateService } from './shared/authenticate.service';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';


describe('AppComponent', () => {

  let authServiceSpy:jasmine.SpyObj<AuthenticateService>;

  beforeEach(() => {
    authServiceSpy=jasmine.createSpyObj('AuthenticateService',['IsloggedIn']);
    TestBed.configureTestingModule({
    imports: [RouterTestingModule,FontAwesomeModule],
    declarations: [AppComponent,NavbarComponent],
    providers:[
      {provide:AuthenticateService, useValue:authServiceSpy},
    ],
  });});

  it('should create the app', () => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.componentInstance;
    expect(app).toBeTruthy();
  });

  it(`should have as title 'EventApp'`, () => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.componentInstance;
    expect(app.title).toEqual('EventApp');
  });

  it('should render title', () => {
    const fixture = TestBed.createComponent(AppComponent);
    fixture.detectChanges();
    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.querySelector('app-navbar')).toBeTruthy();
    expect(compiled.querySelector('router-outlet')).toBeTruthy();
  });
});

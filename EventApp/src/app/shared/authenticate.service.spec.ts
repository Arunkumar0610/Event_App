import { TestBed} from '@angular/core/testing';
import { AuthenticateService } from './authenticate.service';
import { JwtHelperService } from '@auth0/angular-jwt';
describe('AuthenticateService', () => {
 let authService: AuthenticateService;

 beforeEach(() => {
  const spy = jasmine.createSpyObj('JwtHelperService', ['isTokenExpired']);
  TestBed.configureTestingModule({
   providers: [AuthenticateService, { provide: JwtHelperService, useValue: spy }]
  });
  authService = TestBed.inject(AuthenticateService);
 });
 it('should be created', () => {
  expect(authService).toBeTruthy();
 });
});
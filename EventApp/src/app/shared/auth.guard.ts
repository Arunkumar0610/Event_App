import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthenticateService } from './authenticate.service';

export const authGuard: CanActivateFn = (route, state) => {
  const authenticateService = inject(AuthenticateService);
  const router = inject(Router);
  if(authenticateService.IsloggedIn())
  {
    return true;
  }
  else{
    
    router.navigate(["/home"]);
    return false;
  }
  
};
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable({
  providedIn: 'root'
})
export class AuthenticateService {

  constructor(private jwtHelper:JwtHelperService,private router:Router){}
  IsloggedIn(){
    const token=localStorage.getItem("jwt");
    if(token && !this.jwtHelper.isTokenExpired(token)){
      return true;
    }
    localStorage.removeItem("jwt");
    localStorage.removeItem("userName");
    console.log("token not provided or expired");
    return false;
  }  
}

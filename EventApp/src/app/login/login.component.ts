import { Component } from '@angular/core';
import { LoginRequest } from '../shared/LoginRequest';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { EventserviceService } from '../shared/eventservice.service';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { ToastrService } from 'ngx-toastr';
import { LoggingService } from '../shared/logging.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  usercredintials!:LoginRequest
  loginresponse:any
  form=new FormGroup({
  username:new FormControl('',[Validators.required]),
  password:new FormControl('',[Validators.required])
  });

  constructor(private services:EventserviceService,private router:Router,
    private httpClient:HttpClient, private toastr:ToastrService,
    private loggingService:LoggingService) { }

  get username(){return this.form.get('username');}
  get password(){return this.form.get('password');}

  Login()
  {    
    this.usercredintials=new LoginRequest(String(this.username!.value),
    String(this.password!.value));
    console.log(this.usercredintials);
    this.services.LoginUser(this.usercredintials).subscribe(
      {next:(response:any)=>{
        this.loggingService.logInfo(`${this.usercredintials.userName} Login Successful`);
        this.loginresponse=response;  
          localStorage.setItem("jwt",this.loginresponse.token);
          localStorage.setItem("userName",this.usercredintials.userName);
          this.toastr.success('User logged in','Success');
          this.router.navigate(["/home"])
          this.loggingService.logInfo("Navigate to the home page");         
          this.loggingService.logInfo(`Token: ${this.loginresponse.token}`);
          this.loggingService.logInfo(`UserName: ${this.loginresponse.userName}`);    
      },error:(error)=>{
         // Handle login error
     if (error.status === 401) {
      // HTTP status 401 indicates a unauthorized
      this.loggingService.logError("Invalid Username or Password");
      this.toastr.error('Invalid Username or Password','Invalid Credentials');
     } else { 
      this.loggingService.logError("Login failed. Please try again.");
      this.toastr.error('Login failed. Please try again.','Something went wrong');
     }        
      },complete:()=>this.loggingService.logInfo('complete')
  });
  }
  getRouter(){
    return this.router;
  }
}

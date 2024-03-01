import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { User } from '../shared/User';
import { ActivatedRoute, Router } from '@angular/router';
import { EventserviceService } from '../shared/eventservice.service';
import { ToastrService } from 'ngx-toastr';
import { LoggingService } from '../shared/logging.service';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css']
})
export class SignupComponent {
  form=new FormGroup({
    name:new FormControl('',[Validators.required,Validators.maxLength(25)]),
    username:new FormControl('',[Validators.required]),
    password:new FormControl('',[Validators.required,
      Validators.pattern("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[!@#$%^&*()_+=;:<>|./?,-]).{8,15}$")]),
    confirmpassword:new FormControl('',[Validators.required]),
    email:new FormControl('',[Validators.required,Validators.email]),
    contact:new FormControl('',[Validators.required,Validators.pattern("^((\\+91-?)|0)?[0-9]{10}$")])
  }); 
  Users!:User
  compare!:boolean
  constructor(private route:ActivatedRoute,private services:EventserviceService,
    private router:Router,private toastr:ToastrService,
    private loggingService:LoggingService) { }
  ngOnInit(): void {
    this.form.valueChanges.subscribe(frm=>{
      const pass=frm.password;
      const conpass=frm.confirmpassword;
      if(pass!==conpass)
      {
        this.form.get('confirmpassword')?.setErrors({noMatched:true});
      }
      else{
        this.form.get('confirmpassword')?.setErrors(null);
      }
    });
  }
  get name(){return this.form.get('name')}
  get username(){return this.form.get('username')}
  get password(){return this.form.get('password')}
  get confirmpassword(){return this.form.get('confirmpassword')}
  get email(){return this.form.get('email')}
  get contact(){return this.form.get('contact')}

  UserRegistration():void
  {
     this.Users={
      userId:"",
      name:this.name!.value as string,
      userName:this.username!.value as string,
      password:this.password!.value as string,
      confirmPassword:this.confirmpassword!.value as string,
      email:this.email!.value as string,
      contact:this.contact!.value as string,
    }  
    this.services.SignUpUser(this.Users).subscribe({next:(response:any)=>{
        console.log(this.Users);
        console.log("registeredSuccessfully",response);
        this.toastr.success('User Registered Successfull. ','Success');
        this.loggingService.logInfo(`User registered Successfull.`);
        this.router.navigate(['/login'])
      },error:(error)=>{
        console.error('Registration failed',error);    
        if (error.status === 409) {
          // HTTP status 409 indicates a conflict, which often means the username already exists
          this.loggingService.logError(`Username already exists.`);
          this.toastr.error('Username already exists','Failed to SignUp');         
         } else {    
          // Handle other errors    
          // You can customize this part based on the actual error response from your API 
          this.loggingService.logError(`Registration failed. Please try again.`);   
          this.toastr.error('Registration failed. Please try again.',"Something went wrong");  
         }
      },complete:() =>this.loggingService.logInfo("complete")})
  } 
}

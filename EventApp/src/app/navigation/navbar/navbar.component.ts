import { Component } from '@angular/core';
import {  Router } from '@angular/router';
import { faHome , faCalendar,faHeart,faSignOut,faUserCircle,faSearch, faSignIn, faUserPlus} from '@fortawesome/free-solid-svg-icons';
import { AuthenticateService } from 'src/app/shared/authenticate.service';
@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent {
  faHome=faHome;
  faUserPlus=faUserPlus;
  faSignIn=faSignIn;
  faHeart=faHeart;
  faCalendar=faCalendar;
  faSignOut=faSignOut;
  faUserCircle=faUserCircle;
  faSearch=faSearch;
  userName!:string;
  islogin:boolean=false;
  success:boolean=false;
  confirmation:boolean=false;
constructor(private router:Router,private authservice:AuthenticateService){}
ngOnInit():void{
  this.userName=localStorage.getItem("userName") ?? "";
  this.islogin=this.authservice.IsloggedIn();
}
ngAfterContentChecked(): void{
  this.userName=localStorage.getItem("userName") ?? "";
  this.islogin=this.authservice.IsloggedIn();
}
confirmlogout(){
  this.confirmation=true;
  this.success=false;
  if(this.confirmation)
  {
    this.islogin=false;
    localStorage.removeItem("jwt")
    localStorage.removeItem("userName")
    localStorage.removeItem("event")
    this.router.navigate(["/home"]);
  }
}
closeconfirmation()
{
  this.confirmation=false;
}
closesuccess()
{
  this.success=false;
}
Logout(){
  this.success=true; 
}
}


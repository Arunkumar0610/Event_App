import { Component } from '@angular/core';
import { EventserviceService } from '../shared/eventservice.service';
import { ActivatedRoute, Router } from '@angular/router';
import { faPlus } from '@fortawesome/free-solid-svg-icons';
import { ToastrService } from 'ngx-toastr';
import { LoggingService } from '../shared/logging.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent {
  faPlus=faPlus;
  token!:string;
  userName!:string;
  title:string="EventApp";
  Eventlist:any;

  constructor(private route:ActivatedRoute,private router:Router,
    private service:EventserviceService, private toastr:ToastrService,
    private loggingService: LoggingService){  }
  ngOnInit():void
  { 
      this.token=localStorage.getItem("jwt")??"";
      this.userName=localStorage.getItem("userName")??"";
      this.getAllEvents();
  }
  ngAfterContentChecked(): void{
      this.token=localStorage.getItem("jwt")??"";
      this.userName=localStorage.getItem("userName")??"";
  }
  getAllEvents(){
    this.service.GetEvents().subscribe({next:(response:any)=>{
      //console.log("Successfully received event data",response.events)
      this.loggingService.logInfo(`Successfully received event data`);
      this.Eventlist=response.events;
    },
    error:(error)=>{
      //console.error("Error fetching events",error)
      this.loggingService.logError(`Error fetching events`);
    },
  complete:()=>this.loggingService.logInfo('complete')});
  }
  Addtowishlist(events:any){   
    if(this.token!==null && this.token!=='')
    {        
        this.service.AddToWishlist(this.userName,events).subscribe(
          {next:(response:any)=>{
            console.log('Added to wishlist',response);
            this.loggingService.logInfo(`Successfully added event to wishlist`);
            this.toastr.success('Added to wishlist','Success');
          },
          error:(error)=>{
            console.error('Failed adding event',error);
            this.loggingService.logWarn(`Failed adding event`);
            this.toastr.warning('Event already exists in wishlist','Failed');
          },
          complete:()=>console.log('complete')});
    }
    else{
      console.error("Unauthorized access");
      this.loggingService.logError('Unauthorized access');
      this.toastr.error('User login required','Unauthorized');        
      this.router.navigate(["/login"]);
      this.loggingService.logInfo(`Navigate to Login Page`);
    }
  }
  getRouter(){
    return this.router;
  }
}

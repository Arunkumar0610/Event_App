import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { EventserviceService } from '../shared/eventservice.service';
import { faPlus,faSearch } from '@fortawesome/free-solid-svg-icons';
import { ToastrService } from 'ngx-toastr';
import { LoggingService } from '../shared/logging.service';
@Component({
  selector: 'app-events',
  templateUrl: './events.component.html',
  styleUrls: ['./events.component.css']
})
export class EventsComponent {
  faPlus=faPlus;
  faSearch=faSearch;
  token!:string;
  userName!:string;
  title:string="EventApp";
  Eventlist:any;
  EventsFilter:any;
  sear?:string;
  constructor(private route:ActivatedRoute,private router:Router,
    private service:EventserviceService, private toastr:ToastrService,
    private loggingService:LoggingService){  }
  ngOnInit():void
  {
      this.token=localStorage.getItem("jwt")??"";
      this.userName=localStorage.getItem("userName")??"";
      this.getAllEvents();
  }
  getAllEvents(){
    this.service.GetEvents().subscribe({next:(response:any)=>{
      console.log('Successfully received event data',response.events);
      this.loggingService.logInfo(`Successfully received event data.`);
      this.Eventlist=response.events;
      this.EventsFilter=this.Eventlist;
    },error:(error)=>{
      console.error("Error fetching events",error);
      this.loggingService.logError(`Error fetching events.`);
    },
    complete:()=>this.loggingService.logInfo('complete')});
  }

  Addtowishlist(events:any)
  {
    console.log(events);
    this.service.AddToWishlist(this.userName,events).subscribe(
      {next:(response:any)=>{
        console.log('Added to wishlist',response);
        this.loggingService.logInfo(`Successfully added event to wishlist.`);
        this.toastr.success('Added to wishlist','Success');
      },
      error:(error)=>{
        console.error('Failed adding event',error);
        this.loggingService.logWarn(`Failed adding event.`);
        this.toastr.warning('Event already exists in wishlist','Failed');
      },
      complete:()=>this.loggingService.logInfo('complete')})
  }    

  OnSearch(e:any){
    this.sear=e
    this.Eventlist=this.EventsFilter.filter((data:any)=>
    {
      return data.short_title.includes(this.sear)
    })
  }
  }

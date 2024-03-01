import { Component } from '@angular/core';
import { EventserviceService } from '../shared/eventservice.service';
import { ActivatedRoute, Router } from '@angular/router';
import { faSearch, faTrash } from '@fortawesome/free-solid-svg-icons';
import { ToastrService } from 'ngx-toastr';
import { LoggingService } from '../shared/logging.service';
@Component({
  selector: 'app-wishlist',
  templateUrl: './wishlist.component.html',
  styleUrls: ['./wishlist.component.css']
})
export class WishlistComponent {
  faTrash=faTrash;
  faSearch=faSearch;
  token!:string;
  userName!:string;
  title:string="EventApp";
  Eventlist:any;
  EventsFilter:any;
  sear?:string;
  constructor(private route:ActivatedRoute,private router:Router,
    private service:EventserviceService, private toastr:ToastrService,
    private loggerService:LoggingService){ }
  ngOnInit():void
  {
      this.token=localStorage.getItem("jwt")??"";
      this.userName=localStorage.getItem("userName")??"";
      this.getAllWishlistEvents();
  }
  OnSearch(e:any){
    this.sear=e
    this.Eventlist=this.EventsFilter.filter((data:any)=>
    {
      return data.short_title.includes(this.sear)
    })
  }
  getAllWishlistEvents(){
    this.service.GetWishlist(this.userName).subscribe({next:(response:any)=>{
      console.log(response.events);
      this.Eventlist=response.events||[];
      this.EventsFilter=this.Eventlist;
      this.loggerService.logInfo(`Successfully received event data. `);
      console.log('Successfully received event data',this.Eventlist);
    },error:(error) => {
      this.loggerService.logError(`failed fetching data. `);
      console.error('failed fetching data', error);  
     }
    ,complete:()=>console.log("complete")});
  }
  deletefromwishlist(id:number){
        this.service.DeleteWishlist(this.userName,id).subscribe(
          {next:(response:any)=>{
            console.log("Deleted Successfully",response);
            this.loggerService.logInfo(`Successfully removed an event.`);
            this.toastr.success('Removed from wishlist','Success');
            this.getAllWishlistEvents();         
          },
          error:(error)=>{
            this.loggerService.logError(`Failed to delete data. `);
            console.error('Failed to delete data',error);
            this.toastr.error('Event not found in wishlist','Error');          
          },
        complete:()=>this.loggerService.logInfo("complete")}) 
  }
}

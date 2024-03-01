import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { User } from './User';
import { EventItem } from './EventItem';

@Injectable({
  providedIn: 'root'
})
export class EventserviceService {

  private basePath:string="https://localhost:5000/gateway";
  private basePath1:string="https://localhost:5001/api";
  private basePath2:string="https://localhost:5002/api";
  private basePath3:string="https://localhost:5004/api";
  constructor(private http:HttpClient) { }

  public GetEvents():Observable<any>
  {
      return this.http.get(this.basePath+"/Events");
      
  }
  public GetEventById(id:number):Observable<any>
  {
     return this.http.get<EventItem>(this.basePath+"/Events/"+id)
  }
  public SignUpUser(user:User):Observable<any>
  {
    return this.http.post(this.basePath+"/Users",user,{responseType:"json"})
  }
  public LoginUser(credintials:any):Observable<any>
  {
    return this.http.post(this.basePath+"/Authenticate/login",credintials,{responseType:"json"})
    
  }
  public GetWishlist(username:string):Observable<any>
  {
    return this.http.get(this.basePath+"/Wishlist/"+username,{responseType:"json"})
  }
  public AddToWishlist(username:string,eventitem:any):Observable<any>
  {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
    });
    return this.http.post(this.basePath+"/Wishlist/"+username,eventitem,{headers:headers})
  }
  public DeleteWishlist(username:string,id:number):Observable<any>
  {
    return this.http.delete(this.basePath+"/Wishlist/"+username+"/"+id,{responseType:"json"})
  }  
}

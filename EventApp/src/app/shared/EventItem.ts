export class EventItem{
    Id!:number
    Type!:string
    Title!:string
    Short_title?:string
    Datetime_utc?:Date
    Visible_at?:Date|null
    Visible_until?:Date|null
    Venue!:Venue
    Performers: Performer[]=[]
}
export class Venue{
    Name?:string
    Address?:string
    City?:string
    State?:string
    Country?:string
}
export class Performer{
    Id?:number
    Name?:string|null
    Image?:string|null
}
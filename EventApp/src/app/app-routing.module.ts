import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { SignupComponent } from './signup/signup.component';
import { WishlistComponent } from './wishlist/wishlist.component';
import { EventsComponent } from './events/events.component';
import { authGuard } from './shared/auth.guard';

const routes: Routes = [
  {path:"",redirectTo:"home",pathMatch:"full"},
  {path:"home",component:HomeComponent},
  {path:"login",component:LoginComponent},
  {path:"signup",component:SignupComponent},
  {path:"events",component:EventsComponent,canActivate:[authGuard]},
  {path:"wishlist",component:WishlistComponent,canActivate:[authGuard]}
];

@NgModule({
  imports: [RouterModule.forRoot(routes,{scrollPositionRestoration:'top',useHash:true})],
  exports: [RouterModule]
})
export class AppRoutingModule { }

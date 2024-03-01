import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { MatCardModule } from '@angular/material/card';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { SignupComponent } from './signup/signup.component';
import { LoginComponent } from './login/login.component';
import { WishlistComponent } from './wishlist/wishlist.component';
import { NavbarComponent } from './navigation/navbar/navbar.component';
import { TokenInterceptorService } from './shared/token-interceptor.service';
import { JwtHelperService, JwtModule } from '@auth0/angular-jwt';
import { DatePipe, HashLocationStrategy, LocationStrategy } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { EventsComponent } from './events/events.component';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { ToastrModule } from 'ngx-toastr';
import { LoggerModule, NGXLogger, NgxLoggerLevel } from 'ngx-logger';

export function tokenGetter(){
  return localStorage.getItem("jwt");
}

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    SignupComponent,
    LoginComponent,
    WishlistComponent,
    NavbarComponent,
    EventsComponent,
   
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    //FlexLayoutModule,
    AppRoutingModule,
    HttpClientModule,
    MatCardModule,
    //MatSidenavModule,
    //MatToolbarModule,
    //MatIconModule,
    //MatButtonModule,
    //MatListModule,
    FormsModule,
    ReactiveFormsModule,
    FontAwesomeModule,
    //LoggerModule,
    LoggerModule.forRoot({
      //serverLoggingUrl: 'api/logs',
      level: NgxLoggerLevel.DEBUG,
      serverLogLevel: NgxLoggerLevel.ERROR
    }),
    ToastrModule.forRoot({
      timeOut: 5000,
      positionClass: 'toast-bottom-right',
      preventDuplicates: true,
      closeButton:true
    }),
    JwtModule.forRoot({
      config:{
        tokenGetter:tokenGetter,
        allowedDomains:["https://localhost:4200/"],
        disallowedRoutes:[]
      }
    }),
    FontAwesomeModule
  ],
  providers: [{provide:HTTP_INTERCEPTORS,useClass:TokenInterceptorService,multi:true},
    JwtHelperService,DatePipe,NGXLogger,{provide:LocationStrategy,useClass:HashLocationStrategy}],
  bootstrap: [AppComponent]
})
export class AppModule {

 }

import { Injectable } from '@angular/core';
import { NGXLogger } from 'ngx-logger'; // Import NGXLogger
@Injectable({
  providedIn: 'root'
})


export class LoggingService {
  constructor(private logger: NGXLogger) {} // Inject NGXLogger
 
  logInfo(message: string): void {
    console.log(`%c[INFO] -${message}`, `color: blue`);
    this.logger.info(message);
  }
 
  logWarn(message: string): void {
    console.warn(`%c[WARN] -${message}`, `color: orange`);
    this.logger.warn(message);
  }
 
  logError(message: string): void {
    console.error(`%c[ERROR] -${message}`, `color: red`);
    this.logger.error(message);
  }
}

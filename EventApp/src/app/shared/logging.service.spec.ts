import { TestBed } from '@angular/core/testing';
import { NGXLogger } from 'ngx-logger';
import { LoggingService } from './logging.service';

describe('LoggingService', () => {

 let loggingService: LoggingService;
 let loggerSpy: jasmine.SpyObj<NGXLogger>;

 beforeEach(() => {

  const spy = jasmine.createSpyObj('NGXLogger', ['info', 'warn', 'error']);
  TestBed.configureTestingModule({
   providers: [LoggingService, { provide: NGXLogger, useValue: spy }]
  });

  loggingService = TestBed.inject(LoggingService);
  loggerSpy = TestBed.inject(NGXLogger) as jasmine.SpyObj<NGXLogger>;
 });

 it('should be created', () => {
  expect(loggingService).toBeTruthy();
 });

 it('should log info message', () => {
  const message = 'Info Message';
  loggingService.logInfo(message);
  expect(loggerSpy.info).toHaveBeenCalledWith(message);
 });

 it('should log warn message', () => {
  const message = 'Warn Message';
  loggingService.logWarn(message);
  expect(loggerSpy.warn).toHaveBeenCalledWith(message);
 });

 it('should log error message', () => {
  const message = 'Error Message';
  loggingService.logError(message);
  expect(loggerSpy.error).toHaveBeenCalledWith(message);
 });
 
});

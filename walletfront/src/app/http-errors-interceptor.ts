import { HttpInterceptorFn } from '@angular/common/http';
import { catchError } from 'rxjs/operators';
import { throwError } from 'rxjs';

export const httpErrorsInterceptor: HttpInterceptorFn = (req, next) => {
  return next(req).pipe(
    catchError(error =>{
      const simplifiedError = {
        message: 'ocurrio un error'
      }
      return throwError(()=> simplifiedError)
    })
  )
};

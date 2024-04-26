import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { contactSuccessUrl } from "projects/web-portfolio/src/app/core/contants";
import { catchError, map, mergeMap, of } from "rxjs";
import { PorfolioApiService } from "../../../../core/services/porfolio-api.service";
import { sendContactMessage, sendContactMessageFailure, sendContactMessageSuccess } from "./contact.actions";

@Injectable()
export class ContactEffects {

    constructor(private action$: Actions, private portfolioApi: PorfolioApiService, private router: Router) {

    }

    sendContactMessage$ = createEffect(() => this.action$.pipe(
        ofType(sendContactMessage),
        mergeMap((request) => this.portfolioApi.sendContactMessage(request.contactMessage).pipe(
            map(() => {
                this.router.navigateByUrl(contactSuccessUrl);
                return sendContactMessageSuccess({
                    contactMessage: {
                        email: request.contactMessage.email,
                        message: request.contactMessage.message,
                        name: request.contactMessage.name,
                        reason: request.contactMessage.reason
                    }
                });
            }),
            catchError((error) => {
                return of(sendContactMessageFailure({ error }));
            })
        ))
    )
    );
}
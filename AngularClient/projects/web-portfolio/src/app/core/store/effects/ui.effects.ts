import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { catchError, from, map, mergeMap, of, tap } from "rxjs";
import { baseUrl, routeArticleUrl, routeBlogUrl, routeContactsUrl, routeConversationUrl, routeErrorUrl, routeGreetingUrl, routePdfUrl } from "../../contants";
import { PorfolioApiService } from "../../services/porfolio-api.service";
import { showErrorPage, showErrorPageSuccess, showErrorPageFailure, greeting, greetingSuccess, greetingFailure, conversate, conversateSuccess, conversateFailure, showArticle, showArticleSuccess, showArticleFailure, showBlogPost, showBlogPostSuccess, showBlogPostFailure, showPdf, showPdfSuccess, showPdfFailure, contactMe, contactMeSuccess, contactMeFailure, getMainTopics, getMainTopicsSuccess, getMainTopicsFailure, goHome, goHomeSuccess, goHomeFailure } from "../actions/ui.actions";

@Injectable()
export class UiEffects {
    error$ = createEffect(() => this.action$.pipe(
        ofType(showErrorPage),
        mergeMap((error) => {
            return from(this.router.navigate([routeErrorUrl])).pipe(
                map(() => showErrorPageSuccess(error)),
                catchError((error) => of(showErrorPageFailure({ error })))
            )
        })
    ));

    greeting$ = createEffect(() => this.action$.pipe(
        ofType(greeting),
        mergeMap(() => {
            return from(this.router.navigate([routeGreetingUrl])).pipe(
                map(() => greetingSuccess()),
                catchError((error) => of(greetingFailure({ error })))
            )
        })
    ));

    conversate$ = createEffect(() => this.action$.pipe(
        ofType(conversate),
        mergeMap(() => {
            return from(this.router.navigate(
                [routeConversationUrl])).pipe(
                    map(() => conversateSuccess()),
                    catchError((error) => of(conversateFailure({ error })))
                )
        })
    ));

    showarticle$ = createEffect(() => this.action$.pipe(
        ofType(showArticle),
        mergeMap((params) => {
            return from(this.router.navigate([routeConversationUrl]))
                .pipe(
                    map(() => {
                        this.router.navigate([`${routeConversationUrl}`,
                        {
                            outlets: {
                                asidepanel: [routeArticleUrl, params.articleName]
                            }
                        }]);
                        return showArticleSuccess();
                    }),
                    catchError((error) => of(showArticleFailure({ error })))
                )
        })
    ));

    //todo: refactor
    showBlogPost$ = createEffect(() => this.action$.pipe(
        ofType(showBlogPost),
        mergeMap((params) => {
            return from(this.router.navigate([routeConversationUrl]))
                .pipe(
                    map(() => {
                        this.router.navigate([`${routeConversationUrl}`,
                        {
                            outlets: {
                                asidepanel: [routeBlogUrl, params.blogPostName]
                            }
                        }
                        ])
                        return showBlogPostSuccess();
                    }),
                    catchError((error) => of(showBlogPostFailure({ error })))
                )
        })
    ));

    //todo: refactor
    showPdf$ = createEffect(() => this.action$.pipe(
        ofType(showPdf),
        mergeMap((params) => {
            return from(this.router.navigate([routeConversationUrl]))
                .pipe(
                    map(() => {
                        this.router.navigate([`${routeConversationUrl}`,
                        {
                            outlets: {
                                asidepanel: [routePdfUrl, params.pdfName]
                            }
                        }
                        ])
                        return showPdfSuccess();
                    }),
                    catchError((error) => of(showPdfFailure({ error })))
                )
        })
    ));

    //todo: refactor
    contactMe$ = createEffect(() => this.action$.pipe(
        ofType(contactMe),
        mergeMap(() => {
            return from(this.router.navigate([routeConversationUrl]))
                .pipe(
                    map(() => {
                        this.router.navigate([`${routeConversationUrl}`,
                        {
                            outlets: {
                                asidepanel: [routeContactsUrl]
                            }
                        }
                        ])
                        return contactMeSuccess();
                    }),
                    catchError((error) => of(contactMeFailure({ error })))
                )
        })
    ));

    //todo: refactor
    getMainTopics$ = createEffect(() => this.action$.pipe(
        ofType(getMainTopics),
        mergeMap(() => this.portfolioApi.getMainTopics().pipe(
            map((articles) => {
                return getMainTopicsSuccess({ articles });
            }),
            catchError((error) => {
                return of(getMainTopicsFailure({ error }));
            })
        ))
    )
    );

    gohome$ = createEffect(() => this.action$.pipe(
        ofType(goHome),
        mergeMap(() => {
            return from(this.router.navigate([baseUrl])).pipe(
                map(() => goHomeSuccess()),
                catchError((error) => of(goHomeFailure({ error })))
            )
        })
    ));

    constructor(private action$: Actions, private router: Router, private portfolioApi: PorfolioApiService) {

    }
}
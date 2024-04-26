import { Injectable } from "@angular/core";
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { catchError, debounceTime, map, mergeMap, of } from "rxjs";
import { PorfolioApiService } from "../../../../core/services/porfolio-api.service";
import { getArticle, getArticleFailure, getArticleSuccess, getBlogPost, getBlogPostFailure, getBlogPostSuccess, getPdf, getPdfFailure, getPdfSuccess } from "./content.actions";

@Injectable()
export class ContentEffects{
    
    constructor(private action$: Actions, private portfolioApi: PorfolioApiService) {
        
    }

    getArticle$ = createEffect(() => this.action$.pipe(
        ofType(getArticle),
        debounceTime(500),
        mergeMap((article) => this.portfolioApi.getArticle(article.name).pipe(
            map((article) => {
                return getArticleSuccess({article});
            }),
            catchError((error) => {
                return of(getArticleFailure({error}));
            })
        ))
      )
    );

    getBlogPost$ = createEffect(() => this.action$.pipe(
        ofType(getBlogPost),
        debounceTime(500),
        mergeMap((blogPost) => this.portfolioApi.getBlogPost(blogPost.name).pipe(
            map((blogPost) => {
                return getBlogPostSuccess({blogPost});
            }),
            catchError((error) => {
                return of(getBlogPostFailure({error}));
            })
        ))
      )
    );

    getPdf$ = createEffect(() => this.action$.pipe(
        ofType(getPdf),
        debounceTime(500),
        mergeMap((pdf) => this.portfolioApi.getContentUrl(pdf.name).pipe(
            map((contentUrl) => {
                return getPdfSuccess({pdf: contentUrl});
            }),
            catchError((error) => {
                return of(getPdfFailure({error}));
            })
        ))
      )
    );
}
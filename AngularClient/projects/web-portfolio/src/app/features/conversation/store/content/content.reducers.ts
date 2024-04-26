import { createReducer, on } from '@ngrx/store';
import { ConversationState } from '../conversation-state.model';
import { getArticle, getArticleFailure, getArticleSuccess, getBlogPost, getBlogPostFailure, getBlogPostSuccess, getPdf, getPdfFailure, getPdfSuccess } from './content.actions';

export const initialState: ConversationState = {
    isLoading: false
};
export const contentReducer = createReducer(
    initialState,
    on(getArticle, (state) => (
        {
            ...state,
            isLoading: true,
            lastArticle: undefined
        }
    )),
    on(getArticleSuccess, (state, action) => {
        return {
            ...state,
            isLoading: false,
            lastArticle: {...action.article}
        }
    }),
    on(getArticleFailure, (state, action) => (
        {
            ...state,
            isLoading: false,
            error: action.error
        }
    )),
    on(getBlogPost, (state) => (
        {
            ...state,
            isLoading: true,
            lastBlogPost: undefined
        }
    )),
    on(getBlogPostSuccess, (state, action) => (
        {
            ...state,
            isLoading: false,
            lastBlogPost: {...action.blogPost}
        }
    )),
    on(getBlogPostFailure, (state, action) => (
        {
            ...state,
            isLoading: false,
            error: action.error
        }
    )),
    on(getPdf, (state) => (
        {
            ...state,
            isLoading: true,
            lastContentUrl: undefined
        }
    )),
    on(getPdfSuccess, (state, action) => (
        {
            ...state,
            isLoading: false,
            lastContentUrl: {...action.pdf}
        }
    )),
    on(getPdfFailure, (state, action) => (
        {
            ...state,
            isLoading: false,
            error: action.error
        }
    ))
);
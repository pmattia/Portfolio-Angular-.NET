///
/// TO REFACTOR!!!
///
///


import { createReducer, on } from '@ngrx/store';
import { UiState } from '../models/ui-state.model';
import { setMobile as changeTouchGestureAvailability, contactMe, contactMeFailure, contactMeSuccess, conversate, conversateFailure, conversateSuccess, getMainTopics, getMainTopicsFailure, getMainTopicsSuccess, goHome, goHomeFailure, goHomeSuccess, greeting, greetingFailure, greetingSuccess, openSidePanel, showArticle, showArticleFailure, showArticleSuccess, showBlogPost, showBlogPostFailure, showBlogPostSuccess, showErrorPage, showErrorPageFailure, showErrorPageSuccess, showPdf, showPdfFailure, showPdfSuccess } from '../actions/ui.actions';

export const initialState:  UiState = {
  asideShown: false,
  greetingShown: false,
  isLoading: false,
  mainTopics: [],
  isMobile: true
};

export const uiReducer = createReducer(
  initialState,
  on(greeting, (state) => ({...state, isLoading : true})),
  on(greetingSuccess, (state) => ({
    ...state,
    greetingShown: true,
    asideShown: false
  })),
  on(greetingFailure, (state, action) => ({...state, isLoading : false, error: action.error})),
  on(showErrorPage, (state) => ({...state, isLoading : true})),
  on(showErrorPageSuccess, (state) => ({
    ...state,
    isLoading : false
  })),
  on(showErrorPageFailure, (state, action) => ({...state, isLoading : false, error: action.error})),

  on(conversate, (state) => ({...state, isLoading : true})),
  on(conversateSuccess, (state) => ({
    ...state,
    greetingShown: false,
    isLoading : false,
    asideShown: false
  })),
  on(conversateFailure, (state, action) => ({...state, isLoading : false, error: action.error})),

  on(showArticle, (state) => ({
    ...state, 
    isLoading : true,
    asideShown: false
  })),
  on(showArticleSuccess, (state) => ({
    ...state,
    isLoading : false,
    asideShown : true
  })),
  on(showArticleFailure, (state, action) => ({...state, isLoading : false, error: action.error})),
  
  on(showBlogPost, (state) => ({
    ...state,
    isLoading : true,
    asideShown: false
  })),
  on(showBlogPostSuccess, (state) => ({
    ...state,
    isLoading : false,
    asideShown : true
  })),
  on(showBlogPostFailure, (state, action) => ({...state, isLoading : false, error: action.error})),
  
  on(showPdf, (state) => ({
    ...state,
    isLoading : true,
    asideShown: false
  })),
  on(showPdfSuccess, (state) => ({
    ...state,
    isLoading : false,
    asideShown : true
  })),
  on(showPdfFailure, (state, action) => ({...state, isLoading : false, error: action.error})),

  on(contactMe, (state) => ({
    ...state,
    isLoading : true,
    asideShown: false
  })),
  on(contactMeSuccess, (state) => ({
    ...state,
    isLoading : false,
    asideShown: true
  })),
  on(contactMeFailure, (state, action) => ({...state, isLoading : false, error: action.error})),

  on(getMainTopics, (state) => ({...state, isLoading : true})),
  on(getMainTopicsSuccess, (state, action) => ({
    ...state,
    isLoading: false,
    mainTopics: action.articles
  })),
  on(getMainTopicsFailure, (state, action) => ({...state, isLoading : false, error: action.error})),

  on(goHome, (state) => ({...state, isLoading : true})),
  on(goHomeSuccess, (state) => ({
    ...state,
    greetingShown: false,
    asideShown: false,
    isLoading: false
  })),
  on(goHomeFailure, (state, action) => ({...state, isLoading : false, error: action.error})),
  
  on(openSidePanel, (state) => ({...state, asideShown : true})),
  on(changeTouchGestureAvailability, (state, action) => ({...state, isMobile : action.isMobile})),
);
import { createReducer, on } from '@ngrx/store';
import { userAcceptConsent, userRefuseConsent } from '../actions/app.actions';
import { UserState } from '../models/user-state.model';

export const initialAppState:  UserState = {
  userConsent: false
};

export const userReducer = createReducer(
  initialAppState,
  on(userAcceptConsent, (state) => ({
    ...state,
    userConsent: true
  })),
  on(userRefuseConsent, (state) => ({
    ...state,
    userConsent: false
  }))
);
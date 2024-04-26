import { createAction, props } from '@ngrx/store';

export const userAcceptConsent = createAction('[APP] User accept consent');
export const userRefuseConsent = createAction('[APP] User refuse consent');

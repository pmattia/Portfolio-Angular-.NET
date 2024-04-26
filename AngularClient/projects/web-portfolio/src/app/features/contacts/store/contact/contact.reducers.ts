import { createReducer, on } from '@ngrx/store';
import { ContactState } from '../contact-state.model';
import { sendContactMessage, sendContactMessageFailure, sendContactMessageSuccess } from './contact.actions';

export const initialState: ContactState = {
    isLoading: false
};

export const contactReducer = createReducer(
    initialState,
    on(sendContactMessage, (state) => (
        {
            ...state,
            isLoading: true,
            contactMessage: undefined
        }
    )),
    on(sendContactMessageSuccess, (state, model) => (
        {
            ...state,
            isLoading: false,
            contactMessage: model.contactMessage
        }
    )),
    on(sendContactMessageFailure, (state) => (
        {
            ...state,
            isLoading: false
        }
    ))
);
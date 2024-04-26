import { createFeatureSelector, createSelector } from "@ngrx/store";
import { ContactState } from "./contact-state.model";

const selectState = createFeatureSelector<ContactState>('contacts');

export const lastContactMessageSelector = createSelector(selectState, (state) => {
    return state.contactMessage!
});

export const ContactMessageSendingSelector = createSelector(selectState, (state) => {
    return state.isLoading
});
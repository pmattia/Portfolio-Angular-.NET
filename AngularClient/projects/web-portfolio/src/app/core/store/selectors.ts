import { createSelector } from "@ngrx/store";
import { AppState } from "./models/app-state.model";

export const selectState = (state: AppState) => state;
export const isLoadingSelector = createSelector(selectState, (state) => state.ui.isLoading)
export const errorSelector = createSelector(selectState, (state) => state.ui.error)
export const uiSelector = createSelector(selectState, (state) => state.ui)
export const mainTopicsSelector = createSelector(selectState, (state) => state.ui.mainTopics)
export const isMobileSelector = createSelector(selectState, (state) => state.ui.isMobile)
export const userConsent = createSelector(selectState, (state) => state.user.userConsent)   
export const appSelector = createSelector(selectState, (state) => state)
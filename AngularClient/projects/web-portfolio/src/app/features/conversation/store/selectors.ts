import { createFeatureSelector, createSelector } from "@ngrx/store";
import { ConversationState } from "./conversation-state.model";

const selectState = createFeatureSelector<ConversationState>('conversation');

export const lastArticleSelector = createSelector(selectState, (state) => {
    return state.lastArticle!
});
export const lastBlogPostSelector = createSelector(selectState, (state) => {
    return state.lastBlogPost!
});
export const lastContentUrlSelector = createSelector(selectState, (state) => {
    return state.lastContentUrl!
});
export const isContentLoadingSelector = createSelector(selectState, (state) => {
    return state.isLoading
});
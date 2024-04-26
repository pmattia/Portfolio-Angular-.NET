import { ActionReducerMap } from "@ngrx/store";
import { AppState } from "../models/app-state.model";
import { userReducer } from "./user.reducers";
import { uiReducer } from "./ui.reducers";


export const reducers: ActionReducerMap<AppState> = {
    user: userReducer,
    ui: uiReducer
};
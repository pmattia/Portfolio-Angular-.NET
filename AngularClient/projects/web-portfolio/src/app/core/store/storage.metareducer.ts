
import { MetaReducer, ActionReducer } from '@ngrx/store';
import { LocalStorageService } from '../services/local-storage.service';

// the key for the local storage.
const localStorageKey = 'GNAPPO-STORAGE';

export function storageMetaReducer(localStorageService: LocalStorageService): MetaReducer<any> {
    return (reducer: ActionReducer<any>): ActionReducer<any> => {
        let onInit = true;
        return function (state, action) {
            const nextState = reducer(state, action);
    
            if (onInit) {
                onInit = false;
                const savedState = localStorageService.getData(localStorageKey);
                return {...nextState, ...savedState};
            }
            localStorageService.saveData(localStorageKey, nextState);
            return nextState;
        };
     }
   }

import {Action, applyMiddleware, createStore} from 'redux';
import thunk, {ThunkAction} from 'redux-thunk';
import { persistStore, persistReducer } from 'redux-persist'
import storage from 'redux-persist/lib/storage'
import {composeWithDevTools} from 'redux-devtools-extension';

import rootReducer from './reducers';

const persistConfig = {
    key: 'root',
    storage,
    whiteList: ['auth']
};

const persistedReducer = persistReducer(persistConfig, rootReducer);


export const store = createStore(persistedReducer, composeWithDevTools(applyMiddleware((thunk))));
export const persistor = persistStore(store);
export type RootState = ReturnType<typeof rootReducer>;
export type AppThunk<ReturnType=void>=ThunkAction<ReturnType, RootState, unknown, Action<string>>;


import { configureStore, ThunkAction, Action } from '@reduxjs/toolkit';
import counterReducer from '../features/counter/counterSlice';
import profileReducer from '../slices/profileSlice';
import matchReducer from '../slices/matchSlice'
import matchDetailReducer from '../slices/matchDetailSlice'

export const store = configureStore({
  reducer: {
    counter: counterReducer,
    profile: profileReducer,
    match: matchReducer,
    matchDetail: matchDetailReducer,
  },
});

export type AppDispatch = typeof store.dispatch;
export type RootState = ReturnType<typeof store.getState>;
export type AppThunk<ReturnType = void> = ThunkAction<
  ReturnType,
  RootState,
  unknown,
  Action<string>
>;

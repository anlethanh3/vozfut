import { configureStore, ThunkAction, Action } from '@reduxjs/toolkit';
import profileReducer from '../slices/profileSlice';
import memberReducer from '../slices/memberSlice'
import matchReducer from '../slices/matchSlice'
import matchDetailReducer from '../slices/matchDetailSlice'
import newsReducer from '../slices/newsSlice'

export const store = configureStore({
  reducer: {
    profile: profileReducer,
    member: memberReducer,
    match: matchReducer,
    matchDetail: matchDetailReducer,
    news: newsReducer,
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

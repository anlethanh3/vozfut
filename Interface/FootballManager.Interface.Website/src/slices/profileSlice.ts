import { createAsyncThunk, createSlice, PayloadAction } from '@reduxjs/toolkit';
import { RootState } from '../app/store';
import { profile, authenticate } from '../providers/UserApiProvider';
import axios, { AxiosError, HttpStatusCode } from 'axios';

export interface ProfileProps {
  userId: number,
  email: string,
  username: string,
  role: string,
}
export interface TokenProps {
  tokenType: string,
  expiredIn: number,
  accessToken: string,
}

export interface LoginRequestProps {
  email: string | undefined,
  password: string | undefined,
  username?: string | undefined,
}

export interface State {
  token: TokenProps | undefined,
  profile: ProfileProps | undefined,
  status: "loading" | "idle" | "failed",
  error: string | undefined,
  isLoading: boolean,
  isSuccess: boolean,
}

export const initialState: State = {
  token: undefined,
  profile: undefined,
  status: "idle",
  isLoading: false,
  isSuccess: false,
  error: undefined,
}
// The function below is called a thunk and allows us to perform async logic. It
// can be dispatched like a regular action: `dispatch(incrementAsync(10))`. This
// will call the thunk with the `dispatch` function as the first argument. Async
// code can then be executed and other actions can be dispatched. Thunks are
// typically used to make async requests.
export const loginAsync = createAsyncThunk(
  'user/login',
  async (request: LoginRequestProps, { signal }) => {

    return authenticate({ signal: signal, data: request })
      .then(response => {
        if (response.status !== HttpStatusCode.Ok) {
          throw new Error("Unauthorized")
        }
        return response.data
      })
      .catch((ex: Error) => {
        if (ex instanceof AxiosError && ex.response?.status === HttpStatusCode.Unauthorized) {
          throw new Error("Login Failed!")
        }
        throw new Error(ex.message)
      })
  }
)

export const profileAsync = createAsyncThunk(
  'user/profile',
  async (request: TokenProps, { signal }) => {

    return profile({ signal: signal, token: request })
      .then(response => {
        if (response.status !== HttpStatusCode.Ok) {
          throw new Error("Unauthorized")
        }
        return response.data
      })
      .catch((ex: Error) => {
        if (ex instanceof AxiosError && ex.response?.status === HttpStatusCode.Unauthorized) {
          throw new Error("Login Failed!")
        }
        throw new Error(ex.message)
      })
  }
)

export const profileSlice = createSlice({
  name: 'profile',
  initialState,
  // The `reducers` field lets us define reducers and generate associated actions
  reducers: {
    signOut: (state) => {
      state.token = undefined;
      state.profile = undefined;
    },
    onCloseError: (state) => {
      state.error = undefined
    },
    onProfile: (state, action: PayloadAction<ProfileProps>) => {
      state.profile = action.payload
    },
    onToken: (state, action: PayloadAction<TokenProps>) => {
      state.token = action.payload
    },
    onCloseSuccess: (state) => {
      state.isSuccess = false
    }
  },
  // The `extraReducers` field lets the slice handle actions defined elsewhere,
  // including actions generated by createAsyncThunk or in other slices.
  extraReducers: (builder) => {
    builder
      // authenticate
      .addCase(loginAsync.pending, (state) => {
        state.status = 'loading'
        state.isLoading = true
      })
      .addCase(loginAsync.fulfilled, (state, action) => {
        state.status = 'idle'
        state.isLoading = false
        state.isSuccess = true
        state.token = action.payload
      })
      .addCase(loginAsync.rejected, (state, action) => {
        state.status = 'failed'
        state.error = action.error.message
        state.isLoading = false
      })
      // profile
      .addCase(profileAsync.pending, (state) => {
        state.status = 'loading'
        state.isLoading = true
      })
      .addCase(profileAsync.fulfilled, (state, action) => {
        state.status = 'idle'
        state.isLoading = false
        state.isSuccess = true
        state.profile = action.payload
      })
      .addCase(profileAsync.rejected, (state, action) => {
        state.status = 'failed'
        state.error = action.error.message
        state.isLoading = false
      })
  },
});

export const { signOut, onCloseError, onProfile, onToken, onCloseSuccess } = profileSlice.actions;

// The function below is called a selector and allows us to select a value from
// the state. Selectors can also be defined inline where they're used instead of
// in the slice file. For example: `useSelector((state: RootState) => state.counter.value)`
export const selectState = (state: RootState) => state.profile
export const selectProfile = (state: RootState) => state.profile.profile

export default profileSlice.reducer;

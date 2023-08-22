import { createAsyncThunk, createSlice, PayloadAction } from '@reduxjs/toolkit';
import { RootState } from '../app/store';
import { rolling, search } from '../providers/MatchDetailApiProvider';
import { HttpStatusCode } from 'axios';

export interface MatchDetailProps {
  id: number,
  matchId: number,
  matchName: number,
  memberId: number,
  memberName: number,
  isPaid: boolean,
  isSkip: boolean,
  createdDate?: string,
  modifiedDate?: string,
  isDeleted: boolean,
}

export interface PlayerProps {
  id: number,
  elo: number,
  name: string,
}

export interface RollingProps {
  players: PlayerProps[],
  eloSum: number,
}

export interface State {
  data: MatchDetailProps[],
  status: "loading" | "idle" | "failed",
  error: string | undefined,
  isLoading: boolean,
  isShowRivals: boolean,
  rolling: RollingProps[] | undefined,
  search: { name: string }
}
export const initialState: State = {
  data: [],
  status: "idle",
  isLoading: false,
  isShowRivals: false,
  error: undefined,
  rolling: undefined,
  search: { name: '' },
}

export const fetchAsync = createAsyncThunk(
  'matchDetail/fetch',
  async (request: { id: number, }, { signal }) => {
    let { id } = request
    return search<MatchDetailProps[]>({ signal: signal, matchId: id })
      .then(response => {
        if (response.status === HttpStatusCode.Ok) {
          return response.data
        }
        return new Error("Fetch data failed!")
      })
  }
)

export const rollingAsync = createAsyncThunk(
  'matchDetail/rolling',
  async (request: { id: number, }, { signal }) => {
    let { id } = request
    return rolling<RollingProps[]>({ signal: signal, matchId: id })
      .then(response => {
        if (response.status === HttpStatusCode.Ok) {
          return response.data
        }
        return new Error("Rolling data failed!")
      })
  }
)

function isResponseProps(data: MatchDetailProps[] | Error): data is MatchDetailProps[] {
  return (data as MatchDetailProps[]).length !== undefined
}

function isRollingProps(data: RollingProps[] | Error): data is RollingProps[] {
  return (data as RollingProps[]) !== undefined
}

export const matchSlice = createSlice({
  name: 'matchDetail',
  initialState,
  reducers: {
    onCloseError: (state) => {
      state.error = undefined
    },
    onCloseRivals: (state) => {
      state.isShowRivals = false
    }
  },
  extraReducers: (builder) => {
    builder
      .addCase(fetchAsync.pending, (state) => {
        state.status = 'loading'
        state.isLoading = true
      })
      .addCase(fetchAsync.fulfilled, (state, action) => {
        state.status = 'idle'
        state.isLoading = false
        let payload = action.payload
        if (isResponseProps(payload)) {
          state.data = payload
        }
      })
      .addCase(fetchAsync.rejected, (state, action) => {
        state.status = 'failed'
        state.error = action.error.message
        state.isLoading = false
      })
      // rolling
      .addCase(rollingAsync.pending, (state) => {
        state.status = 'loading'
        state.isShowRivals = false
        state.isLoading = true
      })
      .addCase(rollingAsync.fulfilled, (state, action) => {
        state.status = 'idle'
        state.isLoading = false
        state.isShowRivals = true
        let payload = action.payload
        if (isRollingProps(payload)) {
          state.rolling = payload
        }
      })
      .addCase(rollingAsync.rejected, (state, action) => {
        state.status = 'failed'
        state.error = action.error.message
        state.isLoading = false
        state.isShowRivals = false
      })
  },
});

export const { onCloseError, onCloseRivals } = matchSlice.actions;

export const selectState = (state: RootState) => state.matchDetail

export default matchSlice.reducer;

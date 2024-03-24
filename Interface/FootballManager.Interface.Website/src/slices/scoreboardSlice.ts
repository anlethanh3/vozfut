import { createAsyncThunk, createSlice, PayloadAction } from '@reduxjs/toolkit';
import { RootState } from '../app/store';
import { getLeaderboard, getWinner } from '../providers/ScoreboardApiProvider';
import { HttpStatusCode } from 'axios';

export interface LeaderboardProps {
  memberId: number,
  memberName: string,
  goal: number,
  assist: number,
}

export interface WinnerTeamSizeProps {
  memberId: number,
  memberName: string,
  winner: number,
  teamSize: number,
}

export interface State {
  leaderboard: LeaderboardProps[],
  winnerTeam7: WinnerTeamSizeProps[],
  winnerTeam11: WinnerTeamSizeProps[],
  status: "loading" | "idle" | "failed" | "success",
  error: string | undefined,
  isLoading: boolean,
}
export const initialState: State = {
  leaderboard: [],
  winnerTeam7: [],
  winnerTeam11: [],
  status: "idle",
  isLoading: false,
  error: undefined,
}

export const fetchAsync = createAsyncThunk(
  'scoreboard/fetch',
  async (request: {}, { signal }) => {
    let { } = request
    return getLeaderboard<LeaderboardProps[]>({ signal: signal })
      .then(response => {
        if (response.status === HttpStatusCode.Ok) {
          return response.data
        }
        return new Error("Fetch data failed!")
      })
  }
)

export const fetchWinnerTeam7Async = createAsyncThunk(
  'scoreboard/fetchWinner7',
  async (request: { }, { signal }) => {
    let { } = request
    return getWinner<WinnerTeamSizeProps[]>({ teamSize: 7, signal: signal })
      .then(response => {
        if (response.status === HttpStatusCode.Ok) {
          return response.data
        }
        return new Error("Fetch data failed!")
      })
  }
)

export const fetchWinnerTeam11Async = createAsyncThunk(
  'scoreboard/fetchWinner11',
  async (request: {  }, { signal }) => {
    let { } = request
    return getWinner<WinnerTeamSizeProps[]>({ teamSize: 11, signal: signal })
      .then(response => {
        if (response.status === HttpStatusCode.Ok) {
          return response.data
        }
        return new Error("Fetch data failed!")
      })
  }
)

function isResponseProps(data: LeaderboardProps[] | Error): data is LeaderboardProps[] {
  return (data as LeaderboardProps[]).length !== undefined
}

function isWinnerTeamSizeResponseProps(data: WinnerTeamSizeProps[] | Error): data is WinnerTeamSizeProps[] {
  return (data as WinnerTeamSizeProps[]).length !== undefined
}
export const scoreboardSlice = createSlice({
  name: 'scoreboard',
  initialState,
  reducers: {
    onCloseError: (state) => {
      state.error = undefined
    },
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
          state.leaderboard = payload
        }
      })
      .addCase(fetchAsync.rejected, (state, action) => {
        state.status = 'failed'
        state.error = action.error.message
        state.isLoading = false
      })
      .addCase(fetchWinnerTeam7Async.pending, (state) => {
        state.status = 'loading'
        state.isLoading = true
      })
      .addCase(fetchWinnerTeam7Async.fulfilled, (state, action) => {
        state.status = 'idle'
        state.isLoading = false
        let payload = action.payload
        if (isWinnerTeamSizeResponseProps(payload)) {
          state.winnerTeam7 = payload
        }
      })
      .addCase(fetchWinnerTeam7Async.rejected, (state, action) => {
        state.status = 'failed'
        state.error = action.error.message
        state.isLoading = false
      })
      .addCase(fetchWinnerTeam11Async.pending, (state) => {
        state.status = 'loading'
        state.isLoading = true
      })
      .addCase(fetchWinnerTeam11Async.fulfilled, (state, action) => {
        state.status = 'idle'
        state.isLoading = false
        let payload = action.payload
        if (isWinnerTeamSizeResponseProps(payload)) {
          state.winnerTeam11 = payload
        }
      })
      .addCase(fetchWinnerTeam11Async.rejected, (state, action) => {
        state.status = 'failed'
        state.error = action.error.message
        state.isLoading = false
      })
  },
});

export const { onCloseError, } = scoreboardSlice.actions;

export const selectState = (state: RootState) => state.scoreboard

export default scoreboardSlice.reducer;

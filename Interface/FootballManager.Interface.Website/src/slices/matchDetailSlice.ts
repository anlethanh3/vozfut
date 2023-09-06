import { createAsyncThunk, createSlice, PayloadAction } from '@reduxjs/toolkit';
import { RootState } from '../app/store';
import { add, search, remove, updateNew } from '../providers/MatchDetailApiProvider';
import { get as getTeamRival } from '../providers/TeamRivalApiProvider';
import { getId as getMatch } from '../providers/MatchApiProvider';
import { get } from '../providers/MemberApiProvider';
import { HttpStatusCode } from 'axios';
import { MemberProps } from '../slices/memberSlice';
import { MatchProps } from './matchSlice';

export interface MatchDetailProps {
  id: number,
  matchId: number,
  matchName?: number,
  memberId: number,
  memberName?: number,
  isPaid: boolean,
  isSkip: boolean,
  createdDate?: string,
  modifiedDate?: string,
  isDeleted?: boolean,
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
  isShowAdd: boolean,
  members: MemberProps[],
  match: MatchProps | undefined,
  search: { name: string }
}
export const initialState: State = {
  data: [],
  status: "idle",
  isLoading: false,
  isShowRivals: false,
  error: undefined,
  rolling: undefined,
  isShowAdd: false,
  members: [],
  match: undefined,
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

export const fetchMembersAsync = createAsyncThunk(
  'matchDetail/fetchMembers',
  async (id: number, { signal }) => {
    return get<MemberProps[]>({ signal: signal, })
      .then(response => {
        if (response.status === HttpStatusCode.Ok) {
          return response.data
        }
        return new Error("Fetch data failed!")
      })
  }
)

export const addMatchDetailAsync = createAsyncThunk(
  'matchDetail/addMatchDetail',
  async (detail: MatchDetailProps, { signal }) => {
    return add({ data: detail, signal: signal, })
      .then(response => {
        if (response.status === HttpStatusCode.Ok) {
          return response.data
        }
        return new Error("Fetch data failed!")
      })
  }
)

export const deleteMatchDetailAsync = createAsyncThunk(
  'matchDetail/deleteMatchDetail',
  async (id: number, { signal }) => {
    return remove({ id: id, signal: signal, })
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
    return getTeamRival<RollingProps[]>({ signal: signal, id: id })
      .then(response => {
        if (response.status === HttpStatusCode.Ok) {
          return response.data
        }
        return new Error("Rolling data failed!")
      })
  }
)

export const fetchMatchAsync = createAsyncThunk(
  'matchDetail/fetchMatch',
  async (request: { id: number, }, { signal }) => {
    let { id } = request
    return getMatch({ signal: signal, id: id })
      .then(response => {
        if (response.status === HttpStatusCode.Ok) {
          return response.data
        }
        return new Error("Fetch data failed!")
      })
  }
)

export const updateMatchDetailAsync = createAsyncThunk(
  'matchDetail/updateMatchDetail',
  async (request: { data: MatchDetailProps, }, { signal }) => {
    let { data } = request
    return updateNew<MatchDetailProps>({ signal: signal, data: data })
      .then(response => {
        if (response.status === HttpStatusCode.Ok) {
          return response.data
        }
        return new Error("Update new record failed!")
      })
  }
)

function isResponseProps(data: MatchDetailProps[] | Error): data is MatchDetailProps[] {
  return (data as MatchDetailProps[]).length !== undefined
}

function isRollingProps(data: RollingProps[] | Error): data is RollingProps[] {
  return (data as RollingProps[]) !== undefined
}

function isMemberProps(data: MemberProps[] | Error): data is MemberProps[] {
  return (data as MemberProps[]) !== undefined
}

function isMatchDetailProps(data: MatchDetailProps | Error): data is MatchDetailProps {
  return (data as MatchDetailProps) !== undefined
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
    },
    onShowAdd: (state, action: PayloadAction<boolean>) => {
      state.isShowAdd = action.payload
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
      // members
      .addCase(fetchMembersAsync.pending, (state) => {
        state.status = 'loading'
        state.isLoading = true
      })
      .addCase(fetchMembersAsync.fulfilled, (state, action) => {
        state.status = 'idle'
        state.isLoading = false
        let payload = action.payload
        if (isMemberProps(payload)) {
          state.members = payload
        }
      })
      .addCase(fetchMembersAsync.rejected, (state, action) => {
        state.status = 'failed'
        state.error = action.error.message
        state.isLoading = false
      })
      // detail
      .addCase(addMatchDetailAsync.pending, (state) => {
        state.status = 'loading'
        state.isLoading = true
      })
      .addCase(addMatchDetailAsync.fulfilled, (state, action) => {
        state.status = 'idle'
        state.isLoading = false
      })
      .addCase(addMatchDetailAsync.rejected, (state, action) => {
        state.status = 'failed'
        state.error = action.error.message
        state.isLoading = false
      })
      // remove
      .addCase(deleteMatchDetailAsync.pending, (state) => {
        state.status = 'loading'
        state.isLoading = true
      })
      .addCase(deleteMatchDetailAsync.fulfilled, (state, action) => {
        state.status = 'idle'
        state.isLoading = false
      })
      .addCase(deleteMatchDetailAsync.rejected, (state, action) => {
        state.status = 'failed'
        state.error = action.error.message
        state.isLoading = false
      })
      // get match by id
      .addCase(fetchMatchAsync.pending, (state) => {
        state.status = 'loading'
        state.isLoading = true
      })
      .addCase(fetchMatchAsync.fulfilled, (state, action) => {
        state.status = 'idle'
        state.isLoading = false
        state.match = action.payload
      })
      .addCase(fetchMatchAsync.rejected, (state, action) => {
        state.status = 'failed'
        state.error = action.error.message
        state.isLoading = false
      })
      // update new
      .addCase(updateMatchDetailAsync.pending, (state) => {
        state.status = 'loading'
        state.isLoading = true
      })
      .addCase(updateMatchDetailAsync.fulfilled, (state, action) => {
        state.status = 'idle'
        state.isLoading = false
        if (isMatchDetailProps(action.payload)) {
          var item = action.payload
          var index = state.data.findIndex(x => x.matchId === item.matchId && x.memberId === item.memberId)
          if (index != -1) {
            state.data[index] = { ...state.data[index], ...item }
          }
        }
      })
      .addCase(updateMatchDetailAsync.rejected, (state, action) => {
        state.status = 'failed'
        state.error = action.error.message
        state.isLoading = false
      })
  },
});

export const { onCloseError, onCloseRivals, onShowAdd } = matchSlice.actions;

export const selectState = (state: RootState) => state.matchDetail

export default matchSlice.reducer;

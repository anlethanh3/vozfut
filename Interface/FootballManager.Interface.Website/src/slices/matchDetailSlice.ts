import { createAsyncThunk, createSlice, PayloadAction } from '@reduxjs/toolkit';
import { RootState } from '../app/store';
import { search } from '../providers/MatchDetailApiProvider';
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

export interface State {
  data: MatchDetailProps[],
  status: "loading" | "idle" | "failed",
  error: string | undefined,
  isLoading: boolean,
  search: { name: string }
}
export const initialState: State = {
  data: [],
  status: "idle",
  isLoading: false,
  error: undefined,
  search: { name: '' },
}

export const fetchAsync = createAsyncThunk(
  'matchDetail/fetch',
  async (request: { id: number, }, { signal }) => {
    let { id } = request
    return search<MatchDetailProps[]>({ signal: signal, matchId: id })
      .then(response => {
        console.log(response)
        if (response.status === HttpStatusCode.Ok) {
          return response.data
        }
        return new Error("Fetch data failed!")
      })
  }
)

function isResponseProps(data: MatchDetailProps[] | Error): data is MatchDetailProps[] {
  return (data as MatchDetailProps[]).length !== undefined
}

export const matchSlice = createSlice({
  name: 'matchDetail',
  initialState,
  reducers: {
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
  },
});

export const { } = matchSlice.actions;

export const selectState = (state: RootState) => state.matchDetail

export default matchSlice.reducer;

import { createAsyncThunk, createSlice, PayloadAction } from '@reduxjs/toolkit';
import { RootState } from '../app/store';
import { search } from '../providers/MatchApiProvider';
import { HttpStatusCode } from 'axios';

export interface MatchProps {
  id: number,
  name: string,
  description: string,
  teamSize: number,
  teamCount: number,
  createdDate?: string,
  modifiedDate?: string,
  isDeleted: boolean,
}

export interface State {
  data: MatchProps[],
  status: "loading" | "idle" | "failed",
  error: string | undefined,
  isLoading: boolean,
  pageSize: number,
  pageIndex: number,
  totalPage: number,
  search: { name: string }
}
export const initialState: State = {
  data: [],
  status: "idle",
  isLoading: false,
  error: undefined,
  pageIndex: 0,
  totalPage: 0,
  pageSize: 50,
  search: { name: '' },
}

export interface SearchResponseProps<T> {
  data: T,
  pageSize: number,
  pageIndex: number,
  totalPage: number,
}

export const fetchAsync = createAsyncThunk(
  'match/fetch',
  async (request: { name: string, pageSize: number, pageIndex: number, }, { signal }) => {
    let { name, pageSize, pageIndex } = request
    return search<SearchResponseProps<MatchProps[]>>({ signal: signal, name: name, pageIndex: pageIndex, pageSize: pageSize })
      .then(response => {
        if (response.status === HttpStatusCode.Ok) {
          return response.data
        }
        return new Error("Fetch data failed!")
      })
  }
)

function isSearchResponseProps(data: SearchResponseProps<MatchProps[]> | Error): data is SearchResponseProps<MatchProps[]> {
  return (data as SearchResponseProps<MatchProps[]>).pageIndex !== undefined;
}

export const matchSlice = createSlice({
  name: 'match',
  initialState,
  reducers: {
    onChangePageIndex: (state, action: PayloadAction<number>) => {
      state.pageIndex = action.payload
    },
    onChangePageSize: (state, action: PayloadAction<number>) => {
      state.pageSize = action.payload
    }
  },
  extraReducers: (builder) => {
    builder
      // authenticate
      .addCase(fetchAsync.pending, (state) => {
        state.status = 'loading'
        state.isLoading = true
      })
      .addCase(fetchAsync.fulfilled, (state, action) => {
        state.status = 'idle'
        state.isLoading = false
        let payload = action.payload
        if(isSearchResponseProps(payload)){
          state.data = payload.data
          state.pageIndex = payload.pageIndex
          state.pageSize = payload.pageSize
          state.totalPage = payload.totalPage

        }
      })
      .addCase(fetchAsync.rejected, (state, action) => {
        state.status = 'failed'
        state.error = action.error.message
        state.isLoading = false
      })
  },
});

export const { onChangePageIndex, onChangePageSize } = matchSlice.actions;

export const selectState = (state: RootState) => state.match

export default matchSlice.reducer;

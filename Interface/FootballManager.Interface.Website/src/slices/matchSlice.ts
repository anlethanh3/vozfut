import { createAsyncThunk, createSlice, PayloadAction } from '@reduxjs/toolkit';
import { RootState } from '../app/store';
import { add, remove, search, update } from '../providers/MatchApiProvider';
import { HttpStatusCode } from 'axios';
import { RollingProps } from './matchDetailSlice';
import { getAnonymous } from '../providers/TeamRivalApiProvider';

export interface MatchProps {
  id: number,
  name: string,
  description: string,
  teamSize: number,
  teamCount: number,
  createdDate?: string,
  modifiedDate?: string,
  isDeleted?: boolean,
  hasTeamRival?: boolean,
}

export interface State {
  data: MatchProps[],
  status: "loading" | "idle" | "failed",
  error: string | undefined,
  isLoading: boolean,
  pageSize: number,
  pageIndex: number,
  totalPage: number,
  isShowAdd: boolean,
  selectedId: number,
  isShowDelete: boolean,
  isShowUpdate: boolean,
  isShowRivals: boolean,
  rolling: RollingProps[] | undefined,
  search: { name: string },
}
export const initialState: State = {
  data: [],
  status: "idle",
  isLoading: false,
  error: undefined,
  pageIndex: 0,
  totalPage: 0,
  pageSize: 50,
  selectedId: -1,
  search: { name: '' },
  isShowAdd: false,
  isShowUpdate: false,
  isShowDelete: false,
  isShowRivals: false,
  rolling: undefined,
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

export const addAsync = createAsyncThunk(
  'match/add',
  async (data: MatchProps, { signal }) => {
    return add({ signal: signal, data: data })
      .then(response => {
        if (response.status === HttpStatusCode.Ok) {
          return response.data
        }
        return new Error("Add match failed!")
      })
  }
)

export const updateAsync = createAsyncThunk(
  'match/update',
  async (data: MatchProps, { signal }) => {
    return update({ signal: signal, data: data })
      .then(response => {
        if (response.status === HttpStatusCode.Ok) {
          return response.data
        }
        return new Error("Add match failed!")
      })
  }
)

export const deleteAsync = createAsyncThunk(
  'match/delete',
  async (id: number, { signal }) => {
    return remove({ signal: signal, id: id })
      .then(response => {
        if (response.status === HttpStatusCode.Ok) {
          return response.data
        }
        return new Error("Delete match failed!")
      })
  }
)

export const rollingAsync = createAsyncThunk(
  'matchDetail/rolling',
  async (request: { id: number, }, { signal }) => {
    let { id } = request
    return getAnonymous<RollingProps[]>({ signal: signal, id: id })
      .then(response => {
        if (response.status === HttpStatusCode.Ok) {
          return response.data
        }
        return new Error("Rolling data failed!")
      })
  }
)

function isSearchResponseProps(data: SearchResponseProps<MatchProps[]> | Error): data is SearchResponseProps<MatchProps[]> {
  return (data as SearchResponseProps<MatchProps[]>).pageIndex !== undefined;
}
function isRollingProps(data: RollingProps[] | Error): data is RollingProps[] {
  return (data as RollingProps[]) !== undefined
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
    },
    onShowAdd: (state, action: PayloadAction<boolean>) => {
      state.isShowAdd = action.payload
    },
    onShowDelete: (state, action: PayloadAction<boolean>) => {
      state.isShowDelete = action.payload
    },
    onShowUpdate: (state, action: PayloadAction<boolean>) => {
      state.isShowUpdate = action.payload
    },
    onShowTeamRival: (state, action: PayloadAction<boolean>) => {
      state.isShowRivals = action.payload
    },
    onSelectedId: (state, action: PayloadAction<number>) => {
      state.selectedId = action.payload
    },
    onCloseError: (state) => {
      state.error = undefined
    }
  },
  extraReducers: (builder) => {
    builder
      // fetch
      .addCase(fetchAsync.pending, (state) => {
        state.status = 'loading'
        state.isLoading = true
      })
      .addCase(fetchAsync.fulfilled, (state, action) => {
        state.status = 'idle'
        state.isLoading = false
        state.error = undefined
        let payload = action.payload
        if (isSearchResponseProps(payload)) {
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
      // fetch
      .addCase(addAsync.pending, (state) => {
        state.status = 'loading'
        state.isLoading = true
      })
      .addCase(addAsync.fulfilled, (state, action) => {
        state.status = 'idle'
        state.isLoading = false
        state.error = undefined
      })
      .addCase(addAsync.rejected, (state, action) => {
        state.status = 'failed'
        state.error = action.error.message
        state.isLoading = false
      })
      // delete
      .addCase(deleteAsync.pending, (state) => {
        state.status = 'loading'
        state.isLoading = true
      })
      .addCase(deleteAsync.fulfilled, (state, action) => {
        state.status = 'idle'
        state.isLoading = false
        state.error = undefined
      })
      .addCase(deleteAsync.rejected, (state, action) => {
        state.status = 'failed'
        console.log('slice',)
        state.error = action.error.message
        state.isLoading = false
      })

      // update
      .addCase(updateAsync.pending, (state) => {
        state.status = 'loading'
        state.isLoading = true
      })
      .addCase(updateAsync.fulfilled, (state, action) => {
        state.status = 'idle'
        state.isLoading = false
        state.error = undefined
      })
      .addCase(updateAsync.rejected, (state, action) => {
        state.status = 'failed'
        console.log('slice',)
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

export const {
  onChangePageIndex, onChangePageSize,
  onShowAdd, onShowDelete, onShowUpdate,
  onSelectedId, onCloseError,
  onShowTeamRival,
} = matchSlice.actions;

export const selectState = (state: RootState) => state.match

export default matchSlice.reducer;

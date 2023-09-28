import { createAsyncThunk, createSlice, PayloadAction } from '@reduxjs/toolkit';
import { RootState } from '../app/store';
import { HttpStatusCode } from 'axios';
import { add, remove, update, search } from '../providers/MemberApiProvider';

export interface MemberProps {
  id: number,
  name: string,
  realName?: string,
  description?: string,
  elo: number,
  createdDate?: string,
  modifiedDate?: string,
  speed?: number,
  finishing?: number,
  stamina?: number,
  passing?: number,
  skill?: number,
  isDeleted?: boolean,
  championCount?:number,
}
export interface SearchProps {
  name: string,
}
export interface State {
  data: MemberProps[],
  status: string,
  isLoading: boolean,
  error: string | undefined,
  isShowLegend: boolean,
  isShowAdd: boolean,
  isShowUpdate: boolean,
  isShowDelete: boolean,
  selectedId: number,
  pageIndex: number,
  pageSize: number,
  totalPage: number,
  search: SearchProps,
}

export const initialState: State = {
  data: [],
  status: "idle",
  isLoading: false,
  error: undefined,
  isShowLegend: false,
  isShowAdd: false,
  isShowUpdate: false,
  isShowDelete: false,
  selectedId: 0,
  pageIndex: 0,
  pageSize: 50,
  totalPage: 0,
  search: { name: '' }
}
export interface SearchResponseProps<T> {
  data: T,
  pageSize: number,
  pageIndex: number,
  totalPage: number,
}

export const fetchAsync = createAsyncThunk(
  'member/fetch',
  async (request: { name: string, pageSize: number, pageIndex: number, }, { signal }) => {
    let { name, pageSize, pageIndex } = request
    return search<SearchResponseProps<MemberProps[]>>({ signal: signal, name: name, pageIndex: pageIndex, pageSize: pageSize })
      .then(response => {
        if (response.status === HttpStatusCode.Ok) {
          return response.data
        }
        return new Error("Fetch data failed!")
      })
  }
)

export const searchChangedAsync = createAsyncThunk(
  'member/searchChanged',
  async (request: { name: string, pageSize: number, pageIndex: number, }, { signal }) => {
    let { name, pageSize, pageIndex } = request
    return search<SearchResponseProps<MemberProps[]>>({ signal: signal, name: name, pageIndex: pageIndex, pageSize: pageSize })
      .then(response => {
        if (response.status === HttpStatusCode.Ok) {
          return response.data
        }
        return new Error("Fetch data failed!")
      })
  }
)

export const addAsync = createAsyncThunk(
  'member/add',
  async (data: MemberProps, { signal }) => {
    return add({ signal: signal, data: data })
      .then(response => {
        if (response.status === HttpStatusCode.Ok) {
          return response.data
        }
        return new Error("Add Member failed!")
      })
  }
)

export const updateAsync = createAsyncThunk(
  'member/update',
  async (data: MemberProps, { signal }) => {
    return update({ signal: signal, data: data })
      .then(response => {
        if (response.status === HttpStatusCode.Ok) {
          return response.data
        }
        return new Error("Add Member failed!")
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

function isSearchResponseProps(data: SearchResponseProps<MemberProps[]> | Error): data is SearchResponseProps<MemberProps[]> {
  return (data as SearchResponseProps<MemberProps[]>).pageIndex !== undefined;
}

export const memberSlice = createSlice({
  name: 'member',
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
    onShowLegend: (state, action: PayloadAction<boolean>) => {
      state.isShowLegend = action.payload
    },
    onSelectedId: (state, action: PayloadAction<number>) => {
      state.selectedId = action.payload
    },
    onSearchChanged: (state, action: PayloadAction<string>) => {
      state.search.name = action.payload
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
      // add
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
      // search changed
      .addCase(searchChangedAsync.pending, (state) => {
        state.status = 'loading'
        state.isLoading = false
      })
      .addCase(searchChangedAsync.fulfilled, (state, action) => {
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
      .addCase(searchChangedAsync.rejected, (state, action) => {
        state.status = 'failed'
        state.error = action.error.message
        state.isLoading = false
      })
  },
});

export const {
  onChangePageIndex, onChangePageSize,
  onShowAdd, onShowDelete, onShowUpdate,
  onSelectedId, onCloseError,
  onShowLegend, onSearchChanged,
} = memberSlice.actions;

export const selectState = (state: RootState) => state.member

export default memberSlice.reducer;

import { createAsyncThunk, createSlice, PayloadAction } from '@reduxjs/toolkit';
import { RootState } from '../app/store';
import { add, remove, update, search } from '../providers/NewsApiProvider';
import { HttpStatusCode } from 'axios';

export interface NewsProps {
  id: number,
  title: string,
  content: string,
  imageUris: string,
  createdDate?: string,
  modifiedDate?: string,
  isDeleted?: boolean,
}

export interface State {
  data: NewsProps[],
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
}

export interface SearchResponseProps<T> {
  data: T,
  pageSize: number,
  pageIndex: number,
  totalPage: number,
}

export const fetchAsync = createAsyncThunk(
  'news/fetch',
  async (request: { name: string, pageSize: number, pageIndex: number, }, { signal }) => {
    let { name, pageSize, pageIndex } = request
    return search<SearchResponseProps<NewsProps[]>>({ signal: signal, name: name, pageIndex: pageIndex, pageSize: pageSize })
      .then(response => {
        if (response.status === HttpStatusCode.Ok) {
          return response.data
        }
        return new Error("Fetch data failed!")
      })
  }
)

export const addAsync = createAsyncThunk(
  'news/add',
  async (data: NewsProps, { signal }) => {
    return add({ signal: signal, data: data })
      .then(response => {
        if (response.status === HttpStatusCode.Ok) {
          return response.data
        }
        return new Error("Add news failed!")
      })
  }
)

export const updateAsync = createAsyncThunk(
  'news/update',
  async (data: NewsProps, { signal }) => {
    return update({ signal: signal, data: data })
      .then(response => {
        if (response.status === HttpStatusCode.Ok) {
          return response.data
        }
        return new Error("Add news failed!")
      })
  }
)

export const deleteAsync = createAsyncThunk(
  'news/delete',
  async (id: number, { signal }) => {
    return remove({ signal: signal, id: id })
      .then(response => {
        if (response.status === HttpStatusCode.Ok) {
          return response.data
        }
        return new Error("Delete news failed!")
      })
  }
)

function isSearchResponseProps(data: SearchResponseProps<NewsProps[]> | Error): data is SearchResponseProps<NewsProps[]> {
  return (data as SearchResponseProps<NewsProps[]>).pageIndex !== undefined;
}

export const newsSlice = createSlice({
  name: 'news',
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
  },
});

export const {
  onChangePageIndex, onChangePageSize,
  onShowAdd, onShowDelete, onShowUpdate,
  onSelectedId, onCloseError,
} = newsSlice.actions;

export const selectState = (state: RootState) => state.news

export default newsSlice.reducer;

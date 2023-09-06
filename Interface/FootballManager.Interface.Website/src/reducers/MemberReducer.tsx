import { Reducer } from 'react'
export interface MemberProps {
    id: number,
    name: string,
    description?: string,
    elo: number,
    createdDate?: string,
    modifiedDate?: string,
    speed?:number,
    finishing?:number,
    stamina?:number,
    passing?:number,
    skill?:number,
    isDeleted?: boolean,
}
export interface SearchProps {
    name: string,
}
export interface State {
    data: MemberProps[],
    isLoading: boolean,
    error: string | undefined,
    isShowAdd: boolean,
    isShowUpdate: boolean,
    isShowDelete: boolean,
    selectedId: number,
    pageIndex: number,
    pageSize: number,
    totalPage: number,
    search: SearchProps,
}

export const initState: State = {
    data: [],
    isLoading: false,
    isShowAdd: false,
    error: undefined,
    isShowUpdate: false,
    isShowDelete: false,
    selectedId: 0,
    pageIndex: 0,
    pageSize: 10,
    totalPage: 0,
    search: { name: '' }
}

export type Action =
    | { type: 'fetch', }
    | { type: 'success', payload?: any }
    | { type: 'failure', error: string | undefined }
    | { type: 'loading', isLoading: boolean }
    | { type: 'page', pageIndex: number }
    | { type: 'size', pageSize: number }
    | { type: 'showAdd', isShow: boolean }
    | { type: 'showUpdate', isShow: boolean, selectedId: number }
    | { type: 'showDelete', isShow: boolean, selectedId: number }
    | { type: 'search', search: SearchProps }

export const reducer: Reducer<State, Action> = (state, action) => {
    switch (action.type) {
        case 'fetch':
            return { ...state, isLoading: true }
        case 'success':
            return { ...state, isLoading: false, ...action.payload }
        case 'failure':
            return { ...state, isLoading: false, error: action.error }
        case 'loading':
            return { ...state, isLoading: action.isLoading }
        case 'page':
            return { ...state, pageIndex: action.pageIndex }
        case 'size':
            return { ...state, pageSize: action.pageSize }
        case 'showAdd':
            return { ...state, isShowAdd: action.isShow }
        case 'showUpdate':
            return { ...state, isShowUpdate: action.isShow, selectedId: action.selectedId }
        case 'showDelete':
            return { ...state, isShowDelete: action.isShow, selectedId: action.selectedId }
        case 'search':
            return { ...state, search: { ...action.search } }
        default:
            return state;
    }
}
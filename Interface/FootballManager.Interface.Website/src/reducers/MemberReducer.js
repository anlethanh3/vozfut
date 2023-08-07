const initState = {
    data: undefined,
    isLoading: true,
    isShowAdd: false,
    isShowUpdate: false,
    isShowDelete: false,
    selectedId: 0,
    pageIndex: 0,
    pageSize: 10,
    totalPage: undefined,
}

const memberAction = {
    type: 'fetch',
}

const memberReducer = (state, action) => {
    switch (action.type) {
        case 'fetch':
            return { ...state, isLoading: true }
        case 'success':
            return { ...state, isLoading: false, ...action.payload }
        case 'failure':
            return { ...state, isLoading: false }
        case 'page':
            return { ...state, pageIndex: action.pageIndex }
        case 'size':
            return { ...state, pageSize: action.pageSize }
        case 'showAdd':
            return { ...state, isShowAdd: action.isShowAdd }
        case 'showUpdate':
            return { ...state, isShowUpdate: action.isShowUpdate, selectedId: action.selectedId }
        case 'showDelete':
            return { ...state, isShowDelete: action.isShowDelete, selectedId: action.selectedId }
        default:
            return state;
    }
}
export { memberReducer, initState, memberAction }
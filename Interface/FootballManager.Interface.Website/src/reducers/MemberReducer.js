const initState = {
    data: undefined,
    isLoading: true,
    selectedIndex: -1,
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
        default:
            return state;
    }
}
export { memberReducer, initState, memberAction }
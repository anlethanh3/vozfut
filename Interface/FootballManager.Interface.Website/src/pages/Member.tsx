import { useEffect, useReducer } from "react";
import { Button, Col, Row, Table, Pagination, Alert, DropdownButton, Dropdown, } from "react-bootstrap";
import { reducer, initState, MemberProps, State, Action, SearchProps } from "../reducers/MemberReducer";
import { add, remove, update, search } from '../providers/MemberApiProvider'
import moment from 'moment'
import AddMember from "../components/AddMember"
import UpdateMember from "../components/UpdateMember"
import Confirmation from "../components/Confirmation"
import SearchMember from "../components/SearchMember";

export default function Member() {
    const sizes = [10, 50, 100]
    const [state, dispatch] = useReducer(reducer, initState)
    moment.defaultFormat = 'YYYY-MM-DD HH:mm:ss'
    const fetchData = async (signal: AbortSignal) => {
        try {
            dispatch({ type: 'fetch' })
            var data = await search({ pageIndex: state.pageIndex, pageSize: state.pageSize, name: state.search.name, signal: signal },)
            dispatch({ type: 'success', payload: data })
        } catch (ex) {
            dispatch({ type: 'failure', error: ex })
        }
    }
    const onChangePageIndex = (pageIndex: number) => {
        dispatch({ type: 'page', pageIndex: pageIndex })
    }
    const onChangePageSize = (pageSize: number) => {
        dispatch({ type: 'size', pageSize: pageSize })
    }
    const onShowAdd = (isShow: boolean) => {
        dispatch({ type: 'showAdd', isShow: isShow })
    }
    const onShowUpdate = (isShow: boolean, id: number) => {
        dispatch({ type: 'showUpdate', isShow: isShow, selectedId: id })
    }
    const onShowDelete = (isShow: boolean, id: number) => {
        dispatch({ type: 'showDelete', isShow: isShow, selectedId: id })
    }

    const onSubmitAdd = async (member: MemberProps) => {
        dispatch({ type: 'showAdd', isShow: false })
        const abortController = new AbortController()
        await add({ signal: abortController.signal, data: member })
        await fetchData(abortController.signal)
    }
    const onSubmitUpdate = async (member: MemberProps) => {
        dispatch({ type: 'showUpdate', isShow: false, selectedId: member.id })
        const abortController = new AbortController()
        await update({ signal: abortController.signal, data: member })
        await fetchData(abortController.signal)
    }
    const onDelete = async () => {
        dispatch({ type: 'showDelete', isShow: false, selectedId: state.selectedId })
        const abortController = new AbortController()
        await remove({ signal: abortController.signal, id: state.selectedId })
        await fetchData(abortController.signal)
    }
    const onSearch = async (props: { name: string }) => {
        const abortController = new AbortController()
        await fetchData(abortController.signal)
    }

    const onSearchChanged = (search: SearchProps) => {
        dispatch({ type: 'search', search: { ...search } })
    }

    useEffect(() => {
        const abortController = new AbortController()
        fetchData(abortController.signal)
        console.log('effect', state)
        return () => {
            console.log('unmount', state)
            abortController.abort()
        }
    }, [state.pageIndex, state.pageSize])
    return (
        <>
            <h1>Members</h1>
            {
                state.isLoading &&
                <Alert color='primary'>Loading...</Alert>
            }
            {
                state.isShowAdd &&
                <AddMember show={state.isShowAdd} onSubmit={onSubmitAdd} onClose={() => onShowAdd(false)} />
            }
            {
                state.isShowUpdate &&
                <UpdateMember initData={state.data.find(x => x.id === state.selectedId)} show={state.isShowUpdate} onSubmit={onSubmitUpdate} onClose={() => onShowUpdate(false, state.selectedId)} />
            }
            {
                state.isShowDelete &&
                <Confirmation
                    show={state.isShowDelete}
                    title={"Confirmation"}
                    content={"Are you sure you want to permanently delete this member?"}
                    onSubmit={onDelete}
                    onClose={() => onShowDelete(false, 0)} />
            }

            <SearchMember onSearchChanged={onSearchChanged} onSubmit={onSearch} />

            <Row className="my-2">
                <Col className="d-flex justify-content-end">
                    <Button variant="primary" onClick={() => onShowAdd(true)}>Add Member</Button><div className="mx-2" />
                    <Button variant="secondary">Import CSV</Button>
                </Col>
            </Row>

            <Table striped bordered hover>
                <thead>
                    <tr>
                        <th>Id</th>
                        <th>Name</th>
                        <th>Description</th>
                        <th>Elo</th>
                        <th>Modified Date</th>
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {
                        state.data && state.data.map(value =>
                            <tr key={`key-${value.id}`}>
                                <td>{value.id}</td>
                                <td>{value.name}</td>
                                <td>{value.description}</td>
                                <td>+{value.elo}</td>
                                <td>{value.modifiedDate && moment(value.modifiedDate).format()}</td>
                                <td>
                                    <Button variant="warning" onClick={() => onShowUpdate(true, value.id)}>Edit</Button>{' '}
                                    <Button variant="danger" onClick={() => onShowDelete(true, value.id)}>Delete</Button>{' '}
                                </td>
                            </tr>
                        )
                    }
                </tbody>
            </Table>
            {
                state.totalPage &&
                <>
                    <Row>
                        <Col>
                            <DropdownButton className="d-inline" size="lg" variant="secondary" id="dropdown-basic-button" title={state.pageSize}>
                                {
                                    sizes.map(element =>
                                        <Dropdown.Item key={`ditem-${element}`} onClick={() => onChangePageSize(element)}>{element}</Dropdown.Item>
                                    )
                                }
                            </DropdownButton> <p className="d-inline">Records Page {state.pageIndex + 1} of {state.totalPage}</p></Col>
                        <Col>
                            <Pagination className="d-flex justify-content-end">
                                {
                                    Array.from(Array(state.totalPage).keys()).map(element => {
                                        var pageNumber = element + 1
                                        return <Pagination.Item
                                            onClick={() => onChangePageIndex(pageNumber - 1)}
                                            key={`pitem-${element}`}
                                            active={state.pageIndex === pageNumber - 1}>{pageNumber}</Pagination.Item>
                                    })
                                }
                            </Pagination>
                        </Col>
                    </Row>
                </>
            }

        </>
    )
}
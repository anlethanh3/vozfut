import { useEffect, useReducer } from "react";
import { Button, Col, Row, Table, Pagination, Alert, DropdownButton, Dropdown, } from "react-bootstrap";
import { memberReducer, initState } from "../reducers/MemberReducer";
import { paging, add, remove, update } from '../providers/MemberApiProvider'
import moment from 'moment'
import AddMember from "../components/AddMember"
import UpdateMember from "../components/UpdateMember"
import Confirmation from "../components/Confirmation"
import SearchMember from "../components/SearchMember";

function Home() {
    const sizes = [10, 50, 100]
    const [state, dispatch] = useReducer(memberReducer, initState)
    moment.defaultFormat = 'YYYY-MM-DD HH:mm:ss'
    const fetchData = async (signal) => {
        try {
            dispatch({ type: 'fetch' })
            var data = await paging(state.pageIndex, state.pageSize, signal)
            dispatch({ type: 'success', payload: data })
        } catch (ex) {
            dispatch({ type: 'failure', error: ex })
        }
    }
    const onChangePageIndex = (pageIndex) => {
        dispatch({ type: 'page', pageIndex: pageIndex })
    }
    const onChangePageSize = (pageSize) => {
        dispatch({ type: 'size', pageSize: pageSize })
    }
    const onShowAdd = (isShowAdd) => {
        dispatch({ type: 'showAdd', isShowAdd: isShowAdd })
    }
    const onShowUpdate = (isShowUpdate, id) => {
        dispatch({ type: 'showUpdate', isShowUpdate: isShowUpdate, selectedId: id })
    }
    const onShowDelete = (isShowDelete, id) => {
        dispatch({ type: 'showDelete', isShowDelete: isShowDelete, selectedId: id })
    }

    const onSubmitAdd = async (member) => {
        dispatch({ type: 'showAdd', isShowAdd: false })
        const abortController = new AbortController()
        await add(abortController.signal, member)
        await fetchData(abortController.signal)
    }
    const onSubmitUpdate = async (member) => {
        dispatch({ type: 'showUpdate', isShowUpdate: false })
        const abortController = new AbortController()
        await update(abortController.signal, member)
        await fetchData(abortController.signal)
    }
    const onDelete = async () => {
        dispatch({ type: 'showDelete', isShowDelete: false })
        const abortController = new AbortController()
        await remove(abortController.signal, state.selectedId)
        await fetchData(abortController.signal)
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
                <UpdateMember initData={state.data.find(x => x.id === state.selectedId)} show={state.isShowUpdate} onSubmit={onSubmitUpdate} onClose={() => onShowUpdate(false)} />
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

            <SearchMember/>

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
                            <DropdownButton className="d-inline" size="md" variant="secondary" id="dropdown-basic-button" title={state.pageSize}>
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
export default Home;
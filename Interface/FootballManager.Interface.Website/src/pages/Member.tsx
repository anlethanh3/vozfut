import { useEffect, useReducer } from "react";
import { Button, Col, Row, Table, Pagination, Alert, DropdownButton, Dropdown, Toast } from "react-bootstrap";
import { reducer, initState, MemberProps, SearchProps } from "../reducers/MemberReducer";
import { add, remove, update, search } from '../providers/MemberApiProvider'
import moment from 'moment'
import AddMember from "../components/AddMember"
import UpdateMember from "../components/UpdateMember"
import Confirmation from "../components/Confirmation"
import SearchMember from "../components/SearchMember";
import { AxiosError, HttpStatusCode } from "axios";

export default function Member() {
    const sizes = [10, 50, 100]
    const [state, dispatch] = useReducer(reducer, initState)
    moment.defaultFormat = 'YYYY-MM-DD HH:mm:ss'
    const fetchData = async (signal: AbortSignal) => {
        dispatch({ type: 'fetch' })
        search({ pageIndex: state.pageIndex, pageSize: state.pageSize, name: state.search.name, signal: signal },)
            .then(response => {
                if (response.status === 200) {
                    var data = response.data
                    dispatch({ type: 'success', payload: data })
                } else {
                    dispatch({ type: 'failure', error: "Got error while fetch data" })
                }
            })
            .catch((ex: Error) => {
                if (ex instanceof AxiosError && ex.response?.status === HttpStatusCode.Unauthorized) {
                    dispatch({ type: 'failure', error: "Fetch Data Failed!" })
                } else {
                    dispatch({ type: 'failure', error: ex.message })
                }
            })
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
        add({ signal: abortController.signal, data: member })
            .then(response => {
                if (response.status === 200) {
                    fetchData(abortController.signal)
                }
            })
            .catch(error => {
                dispatch({ type: "failure", error: "Access denied! Requested Add!" })
            })
    }
    const onSubmitUpdate = async (member: MemberProps) => {
        dispatch({ type: 'showUpdate', isShow: false, selectedId: member.id })
        const abortController = new AbortController()
        update({ signal: abortController.signal, data: member })
            .then(response => {
                if (response.status === 200) {
                    fetchData(abortController.signal)
                }
            })
            .catch(error => {
                console.log(error)
                dispatch({ type: "failure", error: "Access denied! Requested Update!" })
            })
    }
    const onDelete = async () => {
        dispatch({ type: 'showDelete', isShow: false, selectedId: state.selectedId })
        const abortController = new AbortController()
        remove({ signal: abortController.signal, id: state.selectedId })
            .then(response => {
                if (response.status === 200) {
                    fetchData(abortController.signal)
                }
            })
            .catch(error => {
                console.log(error)
                dispatch({ type: "failure", error: "Access denied! Requested Delete!" })
            })

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

        return function cleanup() {
            console.log('unmount', state)
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
                state.error &&
                <Alert show={state.error !== undefined} variant="danger" onClose={() => dispatch({ type: "failure", error: undefined })} dismissible>
                    {state.error}
                </Alert>
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
                        <th>No</th>
                        <th>Name</th>
                        <th>Nick Name</th>
                        <th>Description</th>
                        <th>Elo</th>
                        <th>Speed</th>
                        <th>Stamina</th>
                        <th>Finishing</th>
                        <th>Passing</th>
                        <th>Skill</th>
                        <th>Modified Date</th>
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {
                        state.data && state.data.map((value,index) =>
                            <tr key={`key-${value.id}`}>
                                <td>{index+1}</td>
                                <td>{value.realName}</td>
                                <td>{value.name}</td>
                                <td>{value.description}</td>
                                <td className="col-sm-1">+{value.elo}</td>
                                <td className="col-sm-1">{value.speed}</td>
                                <td className="col-sm-1">{value.stamina}</td>
                                <td className="col-sm-1">{value.finishing}</td>
                                <td className="col-sm-1">{value.passing}</td>
                                <td className="col-sm-1">{value.skill}</td>
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
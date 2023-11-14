import { Button, Modal, FloatingLabel, Form, Table } from 'react-bootstrap';
import { useState, useEffect } from 'react';
import { MemberProps } from '../slices/memberSlice';

export interface AddClassMemberProps {
    memberIds: number[],
    classId: number,
}

export default function AddMemberClass(props: { show: boolean, members: MemberProps[], onSubmit: (data: AddClassMemberProps) => void, onClose: () => void }) {
    const { show, onSubmit, onClose, members } = props
    const [data, setData] = useState<AddClassMemberProps>({ classId: 2, memberIds: [] })
    const onValid = () => {
        if (data.memberIds.length === 0 || data.classId === -1) {
            return
        }
        onSubmit(data)
    }

    useEffect(() => {
        members.forEach(i => {

        })
    }, [members])

    function onSelected(id: number) {
        let isExist = data.memberIds.includes(id)
        if (!isExist) {
            data.memberIds.push(id)
            setData({ ...data, memberIds: data.memberIds })
        } else {
            setData({ ...data, memberIds: data.memberIds.filter(i => i !== id) })
        }
    }

    return (<>
        <Modal fullscreen show={show} onHide={onClose} backdrop="static">
            <Modal.Header closeButton>
                <Modal.Title>Add Member Class</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                <Form>
                    <Form.Group className="mb-3" controlId="updateRivalMember.ClassId">
                        <FloatingLabel controlId="floatingClassId" label="Team">
                            <Form.Select value={data.classId} onChange={(e) => setData({ ...data, classId: parseInt(e.target.value) })}>
                                <option hidden value={-1}>Select Class</option>
                                {
                                    ["Class SSS", "Class S", "Class A"].map((value, index) =>
                                        <option key={`ClassId.id-${index}`} value={index}>{value}</option>
                                    )
                                }
                            </Form.Select>
                        </FloatingLabel>
                    </Form.Group>
                    <Form.Group className="mb-3" controlId="updateRivalMember.MemberId">
                        <Table striped bordered hover>
                            <thead>
                                <tr>
                                    <th>No</th>
                                    <th>Name</th>
                                    <th>Elo</th>
                                    <th>Choose</th>
                                </tr>
                            </thead>
                            <tbody>
                                {
                                    members && members.map((value, index) => {
                                        var isChoice = false
                                        if (data.memberIds.includes(value.id)) {
                                            isChoice = true
                                        }
                                        return (
                                            <tr key={`key-${index}`}>
                                                <td>{index + 1}</td>
                                                <td>{value.name}</td>
                                                <td>+{value.elo}</td>
                                                <td>
                                                    <Form.Check onChange={(e) => { onSelected(value.id) }} checked={isChoice} type="switch" />
                                                </td>
                                            </tr>
                                        )
                                    }
                                    )
                                }
                            </tbody>
                        </Table>
                    </Form.Group>
                </Form>
            </Modal.Body>
            <Modal.Footer>
                <Button variant="secondary" onClick={onClose}>
                    Close
                </Button>
                <Button variant="primary" onClick={onValid}>
                    Confirm
                </Button>
            </Modal.Footer>
        </Modal>
    </>)
}